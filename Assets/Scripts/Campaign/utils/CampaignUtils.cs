
using UnityEngine;

public static class CampaignUtils
{
    // TODO : launch a campaign with specific character
    public static void LaunchCampaign(CampaignContainer campaign)
    {
        if (campaign != null)
        {
            SceneTransitor.Instance.LoadScene(2, () =>
            {
                GameManager.Instance.SetupCampaign(campaign);
            });
        }
        else
        {
            Debug.LogWarning("Campaign is null in CampaignUtils.LaunchCampaign()");
        }
    }
}
