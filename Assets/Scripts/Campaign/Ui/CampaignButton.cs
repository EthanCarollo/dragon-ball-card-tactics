using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CampaignButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CampaignContainer campaign; 
    public GameObject campaignInfo;
    public Slider campaignAdvancementSlider;
    public TextMeshProUGUI campaignAdvancementText;
    public TextMeshProUGUI campaignName;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (campaign.actualCampaign > campaign.campaign.Length - 1)
        {
            return;
        }
        CampaignUtils.LaunchCampaign(campaign);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        campaignInfo.gameObject.SetActive(true);
        campaignName.text = campaign.campaignName;
        campaignAdvancementSlider.value = campaign.actualCampaign;
        campaignAdvancementSlider.maxValue = campaign.campaign.Length;
        campaignAdvancementText.text = (campaign.actualCampaign / campaign.campaign.Length) * 100 + "%";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        campaignInfo.gameObject.SetActive(false);
    }
}
