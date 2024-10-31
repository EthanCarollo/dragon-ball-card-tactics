using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CampaignButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Campaign campaign;
    public TextMeshProUGUI campaignTextName;
    public Image campaignPreview;

    public void OnPointerClick(PointerEventData eventData)
    {
        GameManager.Instance.campaignManager.SetCampaign(campaign);
        SceneTransitor.Instance.LoadScene(1);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        campaignPreview.gameObject.SetActive(true);
        campaignPreview.sprite = campaign.levels[campaign.levels.Length - 1].characters[0].character.characterSprite;
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        campaignPreview.gameObject.SetActive(false);
    }
}
