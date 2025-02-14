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
    private string repoOwner = "EthanCarollo"; // Replace with the repository owner's username
    private string repoName = "dragon-ball-card-tactics"; // Replace with the repository name
    // This is just a token that is straight forward, it's like JUST JUST to get the archive, he doesn't have any other right
    private string githubPat = "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";

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
            request.SetRequestHeader("User-Agent", "Unity-App");
            request.SetRequestHeader("Authorization", "token " + githubPat);

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
