using System;
using UnityEngine;

public class SceneTransitor : MonoBehaviour
{
    public static SceneTransitor Instance;
    
    public GameObject loadingScreen;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void LoadScene(int sceneToLoad)
    {
        LoadScene(sceneToLoad, null);
    }

    public void LoadScene(int sceneToLoad, Action onEndCallback)
    {
        if (loadingScreen == null)
        {
            Debug.LogError("Cannot load a scene: the loading screen prefab is not configured.");
            return;
        }

        var loadingScreenInstance = Instantiate(loadingScreen);
        var loadingScreenManager = loadingScreenInstance.GetComponentInChildren<LoadingScreenManager>();
        if (loadingScreenManager == null)
        {
            Debug.LogError("Cannot load a scene: the loading screen prefab has no LoadingScreenManager component.");
            Destroy(loadingScreenInstance);
            return;
        }

        loadingScreenManager.StartToLoadScene(sceneToLoad, onEndCallback);
    }
}
