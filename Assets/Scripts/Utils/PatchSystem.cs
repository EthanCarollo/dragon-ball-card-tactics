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
    public string versionFileUrl = "http://localhost:3000/version.txt";
    public string windowsPatchFileUrl = "http://localhost:3000/patch_windows.zip";
    public string macPatchFileUrl = "http://localhost:3000/patch_mac.zip";
    public string localVersionFile = "version.txt";
    public string localPatchPath = "patch.zip";
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI logText;
    public TextMeshProUGUI pathText;
    public TextMeshProUGUI versionText;
    public TextMeshProUGUI progressText;
    public Slider progressBar;
    public Button updateButton;

    private string localVersion;
    private string remoteVersion;

    void Start()
    {
        LogMessage("Persistent Path: " + Application.persistentDataPath);

        // If in the Unity Editor, just enable the button (don't run patch logic)
        if (Application.isEditor)
        {
            EnableUpdateButton();
            statusText.text = "Running in Unity Editor, no patching logic.";
            versionText.text = "version unity editor";
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
    }

    void LogMessage(string message)
    {
        Debug.Log(message);
        logText.text += message + "\n";
    }

    

    IEnumerator CheckForUpdates()
    {
        statusText.text = "Checking for updates...";
        LogMessage("Checking for updates...");
        
        localVersion = GlobalGameConfig.version;
        versionText.text = "version " + localVersion;
        LogMessage("Local version: " + localVersion);
        
        using (UnityWebRequest request = UnityWebRequest.Get(versionFileUrl))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.Success)
            {
                remoteVersion = request.downloadHandler.text.Trim();
                LogMessage("Remote version: " + remoteVersion);
                if (remoteVersion != localVersion)
                {
                    StartCoroutine(DownloadPatch());
                }
                else
                {
                    statusText.text = "Game is up to date!";
                    LogMessage("Game is up to date!");
                    EnableUpdateButton();
                }
            }
            else
            {
                statusText.text = "Failed to check for updates. Please try again later.";
                LogMessage("Failed to check for updates. Error: " + request.error);
                EnableUpdateButton();
            }
        }
    }

    IEnumerator DownloadPatch()
    {
        statusText.text = "Downloading update...";
        LogMessage("Downloading update...");

        string patchFileUrl = Application.platform == RuntimePlatform.WindowsPlayer ? windowsPatchFileUrl : macPatchFileUrl;
        string patchFilePath = Application.persistentDataPath + "/" + localPatchPath;

        using (UnityWebRequest request = UnityWebRequest.Get(patchFileUrl))
        {
            request.downloadHandler = new DownloadHandlerFile(patchFilePath);
            request.SendWebRequest();
            
            while (!request.isDone)
            {
                float progress = request.downloadProgress * 100;
                progressText.text = "Downloading: " + progress.ToString("F2") + "%";
                progressBar.value = request.downloadProgress;
                yield return null;
            }
            
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
        try
        {
            if (!Directory.Exists(extractPath))
            {
                Directory.CreateDirectory(extractPath);
                LogMessage("Creating app directory: " + extractPath);
            }
            using (ZipArchive archive = ZipFile.OpenRead(zipPath))
            {
                foreach (ZipArchiveEntry entry in archive.Entries)
                {
                    string destinationPath = Path.Combine(extractPath, entry.FullName);
                    if (string.IsNullOrEmpty(entry.Name))
                    {
                        Directory.CreateDirectory(destinationPath);
                        LogMessage("Creating directory: " + destinationPath);
                        continue;
                    }
                    string directoryPath = Path.GetDirectoryName(destinationPath);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                        LogMessage("Creating directory: " + directoryPath);
                    }
                    if (File.Exists(destinationPath))
                    {
                        LogMessage("Deleting existing file: " + destinationPath);
                        File.Delete(destinationPath); // Delete old file
                    }
                    entry.ExtractToFile(destinationPath, true);
                    LogMessage("Extracted and replaced file: " + destinationPath);
                    
#if UNITY_STANDALONE_OSX
                    string appExecutablePath = Path.Combine(extractPath, "MacOS/Dragon Ball Card Tactics");  // macOS executable path
                    if (File.Exists(appExecutablePath))
                    {
                        System.Diagnostics.Process.Start("chmod", "+x \"" + appExecutablePath + "\"");
                        LogMessage("Added execute permissions to: " + appExecutablePath);
                        System.Diagnostics.Process.Start("xattr", "-d com.apple.quarantine \"" + appExecutablePath + "\"");
                        LogMessage("Removed quarantine attribute for: " + appExecutablePath);
                    } 
#endif
                }
            }
        }
        catch (Exception error)
        {
            LogMessage("Extraction failed: " + error.Message);
            statusText.text = "Error applying patch: " + error.Message;
            
            // Enable the button so the user can continue playing
            EnableUpdateButton();
        }
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(2);

        string exePath = "";

        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            // Windows: Path to the .exe file
            exePath = Application.dataPath.Replace("_Data", ".exe");
        }
        else if (Application.platform == RuntimePlatform.OSXPlayer)
        {
            // macOS: Path to the app's executable inside the bundle
            exePath = Application.dataPath + "/MacOS/Dragon Ball Card Tactics";  // Replace with your app's name
        }

        try
        {
            // Ensure the path exists before attempting to start the app
            if (File.Exists(exePath))
            {
                System.Diagnostics.Process.Start(exePath);  // Restart the app
                Application.Quit();
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
