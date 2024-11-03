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
}

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