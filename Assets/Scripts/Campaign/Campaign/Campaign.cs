using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

// A campaign is just a composition of multiple Level
[CreateAssetMenu(fileName = "NewCampaign", menuName = "Campaign/Campaign")]
public class Campaign : ScriptableObject
{
    public string campaignName;
    [SerializeField]
    public Level[] levels;

    // The End Campaign function is called when we end a campaign in the game,
    // In that way we can give the player some reward
    public virtual void EndCampaign()
    {
        
    }
}

// A campaign is just a composition of multiple Levels
[Serializable]
public class Level
{
    public CharacterInLevel[] characters;

    [SerializeField] public Dialog[] StartDialog;
}

[Serializable]
public class CharacterInLevel
{
    public CharacterData character;
    public Vector2Int position;
}