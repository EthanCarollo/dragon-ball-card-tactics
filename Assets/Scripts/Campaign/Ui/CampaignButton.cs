using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CampaignButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public CampaignContainer campaign; 
    public GameObject campaignInfo;
    public TextMeshProUGUI campaignName;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        CampaignUtils.LaunchCampaign(campaign.GetActualCampaign());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        campaignInfo.gameObject.SetActive(true);
        campaignName.text = campaign.campaignName;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        campaignInfo.gameObject.SetActive(false);
    }
}
