using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUi : MonoBehaviour
{
    public GameObject campaignMenu;
    
    public void OpenCampaignMenu()
    {
        campaignMenu.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(-Screen.width, 0);
        campaignMenu.SetActive(true);
        LeanTween.moveX(campaignMenu.transform.GetComponent<RectTransform>(), 0f, 0.5f)
            .setEase(LeanTweenType.easeOutCirc);
    }

    public void QuitCampaignMenu()
    {
        LeanTween.moveX(campaignMenu.transform.GetComponent<RectTransform>(), -Screen.width, 0.5f)
            .setEase(LeanTweenType.easeOutCirc)
            .setOnComplete(f => campaignMenu.SetActive(false));
    }
}