using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class TrevianumDownloader : EditorWindow
{
    [MenuItem("Trevi/Download Trevianum Photospheres")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(TrevianumDownloader));
    }

    void OnGUI()
    {
        if (GUILayout.Button("Download Photospheres"))
        {
            StartDownloads();
        }
    }

    private void StartDownloads()
    {
        Queue<DownloadJob> downloadQueue = new Queue<DownloadJob>();
        string[] panoramaTemplates = File.ReadAllLines("photosphere_urls.txt");
        foreach (string template in panoramaTemplates)
        {
            if (!string.IsNullOrEmpty(template.Trim()))
            {
                ParsePanoramaTemplate(template.Trim(), downloadQueue);
            }
        }

        if (downloadQueue.Count > 0)
        {
            DownloaderUtility.StartDownloadQueue(downloadQueue);
        }
    }

    private void ParsePanoramaTemplate(string pathTemplate, Queue<DownloadJob> queue)
    {
        // Regex to extract panorama name and resolution from templates like:
        // media/panorama_.../{face}/0/{row}_{column}.jpg
        Regex regex = new Regex(@"media/(panorama_[\w]+)/{face}/(\d)/{row}_{column}\.jpg");
        Match match = regex.Match(pathTemplate);

        if (!match.Success)
        {
            Debug.LogWarning("Skipping malformed URL template: " + pathTemplate);
            return;
        }

        string panoramaName = match.Groups[1].Value;
        int resolutionLevel = int.Parse(match.Groups[2].Value);

        int rowCount;
        int colCount;

        switch (resolutionLevel)
        {
            case 0: rowCount = 7; colCount = 42; break;
            case 1: rowCount = 4; colCount = 24; break;
            case 2: rowCount = 2; colCount = 12; break;
            case 3: rowCount = 1; colCount = 6; break;
            default:
                Debug.LogWarning("Unknown resolution level: " + resolutionLevel + " in template: " + pathTemplate);
                return;
        }

        string destDir = Path.Combine("Assets/StreamingAssets/TrevianumPhotospheres", panoramaName, resolutionLevel.ToString());

        for (int i = 0; i < 6; i++)
        {
            string face = "f";
            switch (i)
            {
                case 1: face = "b"; break;
                case 2: face = "l"; break;
                case 3: face = "r"; break;
                case 4: face = "u"; break;
                case 5: face = "d"; break;
            }

            for (int row = 0; row < rowCount; row++)
            {
                for (int col = 0; col < colCount; col++)
                {
                    // The path template already has the resolution level, so we just replace the other parts.
                    string tileUrlPath = pathTemplate.Replace("{face}", face).Replace("{row}", row.ToString()).Replace("{column}", col.ToString());
                    string fullUrl = "https://naartrevianum.nl/" + tileUrlPath;
                    string destPath = Path.Combine(destDir, face, row + "_" + col + ".jpg");
                    if (!File.Exists(destPath))
                    {
                        queue.Enqueue(new DownloadJob(fullUrl, destPath));
                    }
                }
            }
        }
    }

    void OnDestroy()
    {
        DownloaderUtility.CancelDownloads();
    }
}
