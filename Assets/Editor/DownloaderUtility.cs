using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ComponentModel;

public struct DownloadJob
{
    public string Url;
    public string DestinationPath;

    public DownloadJob(string url, string destPath)
    {
        Url = url;
        DestinationPath = destPath;
    }
}

public static class DownloaderUtility
{
    private static Queue<DownloadJob> downloadQueue = new Queue<DownloadJob>();
    private static bool isDownloading = false;
    private static int totalFiles = 0;
    private static int filesDownloaded = 0;
    private static WebClient currentWebClient;

    public static void StartDownloadQueue(Queue<DownloadJob> jobs)
    {
        if (isDownloading)
        {
            Debug.LogWarning("A download is already in progress.");
            return;
        }

        downloadQueue = new Queue<DownloadJob>(jobs); // Copy the queue

        if (downloadQueue.Count > 0)
        {
            isDownloading = true;
            totalFiles = downloadQueue.Count;
            filesDownloaded = 0;
            DownloadNextFile();
        }
    }

    private static void DownloadNextFile()
    {
        if (downloadQueue.Count > 0)
        {
            DownloadJob job = downloadQueue.Dequeue();
            Directory.CreateDirectory(Path.GetDirectoryName(job.DestinationPath));

            currentWebClient = new WebClient();
            currentWebClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
            currentWebClient.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadCompletedCallback);

            try
            {
                currentWebClient.DownloadFileAsync(new System.Uri(job.Url), job.DestinationPath, job);
                filesDownloaded++;
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Failed to start download for " + job.Url + ": " + ex.Message);
                DownloadNextFile(); // Try next file
            }
        }
        else
        {
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("Download Complete", "All files have been downloaded.", "OK");
            isDownloading = false;
        }
    }

    private static void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
    {
        DownloadJob job = (DownloadJob)e.UserState;
        string progressMessage = $"Downloading file {filesDownloaded} of {totalFiles}: {Path.GetFileName(job.Url)} ({e.BytesReceived / 1024} KB / {e.TotalBytesToReceive / 1024} KB)";
        float progress = ((float)(filesDownloaded -1) / totalFiles) + ((float)e.ProgressPercentage / 100 / totalFiles);
        EditorUtility.DisplayProgressBar("Downloading Assets...", progressMessage, progress);
    }

    private static void DownloadCompletedCallback(object sender, AsyncCompletedEventArgs e)
    {
        if (e.Cancelled)
        {
            Debug.LogError("Download was canceled.");
        }
        else if (e.Error != null)
        {
            DownloadJob job = (DownloadJob)e.UserState;
            Debug.LogError("Failed to download " + job.Url + ": " + e.Error.ToString());
        }

        currentWebClient.Dispose();
        currentWebClient = null;

        DownloadNextFile();
    }

    public static void CancelDownloads()
    {
        if (currentWebClient != null)
        {
            currentWebClient.CancelAsync();
        }
        downloadQueue.Clear();
        isDownloading = false;
        EditorUtility.ClearProgressBar();
    }
}
