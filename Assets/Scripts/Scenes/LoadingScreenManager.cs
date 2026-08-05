using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenManager : MonoBehaviour
{
    public Image loadingScreenImage;
    [SerializeField]
    private Slider loadingSlider;
    private bool _sceneIsSwapping;

    public void StartToLoadScene(int sceneToLoad)
    {
        StartToLoadScene(sceneToLoad, null);
    }

    public void StartToLoadScene(int sceneToLoad, Action onEndCallback)
    {
        if (_sceneIsSwapping)
        {
            return;
        }

        if (sceneToLoad < 0 || sceneToLoad >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogError($"Cannot load scene at build index {sceneToLoad}: index is outside the build settings.");
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        StartCoroutine(LoadScene(sceneToLoad, onEndCallback));
    }

    private IEnumerator LoadScene(int sceneToLoad, Action onEndCallback)
    {
        _sceneIsSwapping = true;
        float startPosition = loadingScreenImage == null
            ? 0f
            : loadingScreenImage.rectTransform.position.y;

        if (loadingScreenImage != null)
        {
            LeanTween.moveY(loadingScreenImage.rectTransform, 0, 1f)
                .setEase(LeanTweenType.easeOutQuart)
                .setIgnoreTimeScale(true);
            yield return new WaitForSecondsRealtime(1f);
        }

        AsyncOperation asyncSceneToLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        if (asyncSceneToLoad == null)
        {
            Debug.LogError($"Unity could not start loading scene at build index {sceneToLoad}.");
            FinishLoading();
            yield break;
        }

        asyncSceneToLoad.allowSceneActivation = false;

        while (asyncSceneToLoad.progress < 0.9f)
        {
            if (loadingSlider != null)
            {
                loadingSlider.value = asyncSceneToLoad.progress;
            }

            yield return new WaitForEndOfFrame();
        }

        if (loadingSlider != null)
        {
            loadingSlider.value = 1f;
        }

        asyncSceneToLoad.allowSceneActivation = true;
        yield return new WaitForEndOfFrame();
        yield return new WaitForFixedUpdate();

        try
        {
            onEndCallback?.Invoke();
        }
        catch (Exception callbackException)
        {
            Debug.LogException(callbackException);
        }

        yield return new WaitForSecondsRealtime(0.2f);
        if (loadingScreenImage != null)
        {
            LeanTween.moveY(loadingScreenImage.rectTransform, -startPosition, 1f)
                .setEase(LeanTweenType.easeInQuart)
                .setIgnoreTimeScale(true);
            yield return new WaitForSecondsRealtime(1.2f);
        }

        FinishLoading();
    }

    private void FinishLoading()
    {
        _sceneIsSwapping = false;
        Destroy(this.gameObject);
    }
}
