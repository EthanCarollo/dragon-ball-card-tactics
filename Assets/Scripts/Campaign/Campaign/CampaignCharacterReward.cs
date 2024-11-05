using UnityEngine;

[CreateAssetMenu(fileName = "NewCampaignCharacterReward", menuName = "Campaign/CampaignCharacterReward")]
public class CampaignCharacterReward : Campaign
{
    public CharacterData rewardCharacter;

    public override void EndCampaign()
    {
        GameManager.Instance.characterInventory.AddCharacter(rewardCharacter);
    }
}