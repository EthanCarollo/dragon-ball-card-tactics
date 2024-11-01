using UnityEngine;
using UnityEngine.EventSystems;

public class CampaignButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        CampaignUtils.LaunchCampaign(CampaignDatabase.Instance.campaigns[0]);
    }
}
