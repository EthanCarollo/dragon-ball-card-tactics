using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PatchSystem : MonoBehaviour
{
    public string localVersionFile = "version.txt";
    public string localPatchPath = "patch.zip";
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI logText;
    public TextMeshProUGUI pathText;
    public TextMeshProUGUI versionText;
    public TextMeshProUGUI progressText;
    public Slider progressBar;
    public Button updateButton;

    public TextMeshProUGUI patchNoteText;

    private string localVersion;
    private string remoteVersion;

    private string repoOwner = "EthanCarollo"; // Replace with the repository owner's username
    private string repoName = "dragon-ball-card-tactics"; // Replace with the repository name
    // This is just a token that is straight forward, it's like JUST JUST to get the archive, he doesn't have any other right
    private string githubPat = "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";


    void Start()
    {
        patchNoteText.text = GlobalGameConfig.patchNotes;
        LogMessage("Persistent Path: " + Application.persistentDataPath);

        if (Application.isEditor)
        {
            EnableUpdateButton();
            statusText.text = "Running in Unity Editor, no patching logic.";
            versionText.text = "version unity editor (" + GlobalGameConfig.version + ")";
            return;
        }

        try {
            StartCoroutine(CheckLatestVersion());
        } catch (Exception error) {
            LogMessage("---- ERROR MESSAGE ----");
            LogMessage("---- ERROR MESSAGE ----");
            LogMessage("---- ERROR MESSAGE ----");
            LogMessage(error.ToString());
            LogMessage("---- ERROR MESSAGE ----");
            LogMessage("---- ERROR MESSAGE ----");
            LogMessage("---- ERROR MESSAGE ----");
            EnableUpdateButton();
        }
        /*
        // If in the Unity Editor, just enable the button (don't run patch logic)
        if (Application.isEditor)
        {
            EnableUpdateButton();
            statusText.text = "Running in Unity Editor, no patching logic.";
            versionText.text = "version unity editor (" + GlobalGameConfig.version + ")";
        }
        else
        {
            try
            {
                StartCoroutine(CheckForUpdates());
            }
            catch (Exception error)
            {
                statusText.text = error.Message;
            }
        }
        */
    }

    void LogMessage(string message)
    {
        Debug.Log(message);
        logText.text += message + "\n";
    }

    private IEnumerator CheckLatestVersion()
    {
        string url = $"https://api.github.com/repos/{repoOwner}/{repoName}/releases/latest";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("User-Agent", "Unity-App");
            request.SetRequestHeader("Authorization", "token " + githubPat);

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                GitHubRelease release = JsonUtility.FromJson<GitHubRelease>(json);
                Debug.Log("Latest Version: " + release.tag_name);
                var wantedRelease = Application.platform == RuntimePlatform.WindowsPlayer ? 
                    "DragonBallCardTactics-StandaloneWindows64.zip" : "DragonBallCardTactics-StandaloneOSX.zip";
                foreach(var asset in release.assets){
                    if(asset.name == wantedRelease){
                        LogMessage(asset.browser_download_url);
                        StartCoroutine(DownloadPatch(asset.url));
                    }
                }
            }
            else
            {
                Debug.LogError("Failed to fetch latest version: " + request.error);
            }
        }
    }

    IEnumerator DownloadPatch(string url)
    {
        statusText.text = "Downloading update...";
        
        LogMessage("Local version: " + localVersion);
        LogMessage("Remote version: " + remoteVersion);
        LogMessage("Downloading update...");
        LogMessage("Download URL : " + url);

        string patchFilePath = Application.persistentDataPath + "/" + localPatchPath;

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.SetRequestHeader("User-Agent", "Unity-App");
            request.SetRequestHeader("Authorization", "token " + githubPat);
            request.SetRequestHeader("Accept", "application/octet-stream");

            request.downloadHandler = new DownloadHandlerFile(patchFilePath);
            request.SendWebRequest();
            
            while (!request.isDone)
            {
                float progress = request.downloadProgress * 100;
                progressText.text = "Downloading: " + progress.ToString("F2") + "%";
                progressBar.value = request.downloadProgress;
                yield return null;
            }
            progressText.text = "Downloading: " + 100.ToString("F2") + "%";
            progressBar.value = progressBar.maxValue;
            
            if (request.result == UnityWebRequest.Result.Success)
            {
                LogMessage("Download complete, applying patch...");
                ApplyPatch(patchFilePath);
            }
            else
            {
                statusText.text = "Failed to download patch: " + request.error;
                LogMessage("Failed to download patch: " + request.error);
                EnableUpdateButton();
            }
        }
    }


    

    void ApplyPatch(string patchFilePath)
    {
        statusText.text = "Applying update...";
        LogMessage("Applying update...");
        try
        {
            string appPath = Directory.GetParent(Application.dataPath.Replace("_Data", "")).FullName;
            
#if UNITY_STANDALONE_OSX
            appPath = Application.dataPath;
#endif

            if (File.Exists(patchFilePath) && new FileInfo(patchFilePath).Length > 0)
            {
                ExtractAndOverwrite(patchFilePath, appPath);
                File.Delete(patchFilePath);
                statusText.text = "Update complete! Restarting...";
                LogMessage("Update complete! Restarting...");
                StartCoroutine(RestartGame());
            }
            else
            {
                statusText.text = "Patch file is missing or empty.";
                LogMessage("Patch file is missing or empty.");
                EnableUpdateButton();
            }
        }
        catch (Exception error)
        {
            Debug.LogError("Error applying patch: " + error);
            statusText.text = "Error applying patch: " + error.Message;
            LogMessage("Error applying patch: " + error.Message);
            EnableUpdateButton();
        }
    }

    void ExtractAndOverwrite(string zipPath, string extractPath)
    {
        LogMessage(zipPath);
        LogMessage(extractPath);

        try
        {
            // Log the paths for reference
            LogMessage($"ZIP Path: {zipPath}");
            LogMessage($"Extract Path: {extractPath}");

            Directory.CreateDirectory(extractPath);
            ZipFile.ExtractToDirectory(zipPath, extractPath, true); 

            LogMessage("Extraction complete.");
        }
        catch (Exception ex)
        {
            LogMessage($"Error: {ex.Message}");
        }
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(2);

        string exePath = "";

        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            exePath = Application.dataPath.Replace("_Data", ".exe");
        }
        else if (Application.platform == RuntimePlatform.OSXPlayer)
        {
            exePath = Application.dataPath + "/MacOS/Dragon Ball Card Tactics";
        }

        try
        {
            if (File.Exists(exePath))
            {
                // System.Diagnostics.Process.Start(exePath);  // Restart the app
                // Application.Quit();
            }
            else
            {
                LogMessage("Executable not found at path: " + exePath);
                statusText.text = "Error restarting: Executable not found at " + exePath;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Error restarting: " + ex.Message);
            statusText.text = "Error restarting: " + ex.Message;
        }
    }

    // This method enables the update button when the game is up to date or when an error occurs
    void EnableUpdateButton()
    {
        updateButton.gameObject.SetActive(true);
        updateButton.interactable = true;
        LogMessage("Update button enabled!");
    }
}

[Serializable]
public class GitHubRelease
{
    public string tag_name;
    public GitHubAssets[] assets;
}

[Serializable]
public class GitHubAssets
{
    public string url;
    public string name;
    public string browser_download_url;
}