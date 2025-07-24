using System;
using System.Collections;
using System.IO;
using System.IO.Compression;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CheckVersionText : MonoBehaviour
{
    public TextMeshProUGUI text;
    private string repoOwner = "EthanCarollo";
    private string repoName = "dragon-ball-card-tactics";

    private void Start()
    {
        try
        {
            StartCoroutine(CheckLatestVersion());
        }
        catch (Exception e)
        {
            Debug.Log(e);
            text.text = "Cannot check version of game online";
        }
    }

    private IEnumerator CheckLatestVersion()
    {
        string url = $"https://api.github.com/repos/{repoOwner}/{repoName}/releases/latest";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                GitHubRelease release = JsonUtility.FromJson<GitHubRelease>(json);
                Debug.Log("Latest Version: " + release.tag_name);
                if (release.tag_name != ("v" + GlobalGameConfig.version))
                {
                    text.text = $"New version of the game is available : {release.tag_name}";
                }
                else
                {
                    text.text = "Game is up to date";
                }
            }
            else
            {
                Debug.LogError("Failed to fetch latest version: " + request.error);
            }
        }
    }

}

[Serializable]
public class GitHubRelease
{
    public string tag_name;
}
