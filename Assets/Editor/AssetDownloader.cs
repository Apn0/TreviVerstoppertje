using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class AssetDownloader : EditorWindow
{
    private string urlList = "";
    private string downloadPath = "Assets/StreamingAssets/Downloads";

    [MenuItem("Trevi/Download Assets")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(AssetDownloader));
    }

    void OnGUI()
    {
        GUILayout.Label("Asset Downloader", EditorStyles.boldLabel);
        downloadPath = EditorGUILayout.TextField("Download Path", downloadPath);
        urlList = EditorGUILayout.TextArea(urlList, GUILayout.Height(100));

        if (GUILayout.Button("Download"))
        {
            StartDownloads();
        }
    }

    private void StartDownloads()
    {
        Queue<DownloadJob> downloadQueue = new Queue<DownloadJob>();
        string[] urls = urlList.Split('\n');
        foreach (string url in urls)
        {
            if (!string.IsNullOrEmpty(url.Trim()))
            {
                string fileName = Path.GetFileName(url.Trim());
                string destPath = Path.Combine(downloadPath, fileName);
                downloadQueue.Enqueue(new DownloadJob(url.Trim(), destPath));
            }
        }

        if (downloadQueue.Count > 0)
        {
            DownloaderUtility.StartDownloadQueue(downloadQueue);
        }
    }

    void OnDestroy()
    {
        DownloaderUtility.CancelDownloads();
    }
}
