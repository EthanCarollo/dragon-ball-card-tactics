
using UnityEngine;

public static class CampaignUtils
{
    // TODO : launch a campaign with specific character
    public static void LaunchCampaign(Campaign campaign)
    {
        if (campaign != null)
        {
            SceneTransitor.Instance.LoadScene(0);
            GameManager.Instance.SetupGameBoardForLevel(campaign.levels[0], new CharacterContainer[]
            {
                GameManager.Instance.characterInventory.characters[0]
            });
        }
        else
        {
            Debug.LogWarning("Campaign is null in CampaignUtils.LaunchCampaign()");
        }
    }
}
