using System;
using UnityEngine;
using UnityEngine.UI;

public class CampaignMenuUi : MonoBehaviour
{
    public GameObject campaignButton;
    public Image previewImage;
    
    public void Start()
    {
        InstantiateCampaignButton();
    }

    public void InstantiateCampaignButton()
    {
        if (CampaignDatabase.Instance.campaigns != null && CampaignDatabase.Instance.campaigns.Length > 0)
        {
            foreach (Campaign campaign in CampaignDatabase.Instance.campaigns)
            {
                GameObject instance = Instantiate(campaignButton, transform);
                CampaignButton button = instance.GetComponent<CampaignButton>();
                button.campaign = campaign;
                button.campaignTextName.text = campaign.campaignName;
                button.campaignPreview = previewImage;
            }
        }
    }
}
