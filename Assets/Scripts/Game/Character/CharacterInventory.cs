using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInventory", menuName = "Character/CharacterInventory")]
public class CharacterInventory : ScriptableObject
{
    public List<CharacterContainer> characters = new List<CharacterContainer>();
    
    // this is the index from the character array that is used for a fight
    public int[] selectedIndexCharacterForCampaign;

    private static CharacterInventory _instance;
    public static CharacterInventory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<CharacterInventory>("CharacterInventory");
                if (_instance == null)
                {
                    Debug.LogError("CampaignDatabase instance not found in Resources folder!");
                }
            }
            return _instance;
        }
    }

    public void VerifyCharacterSelected()
    {
        for (int i = 0; i < selectedIndexCharacterForCampaign.Length; i++)
        {
            if (selectedIndexCharacterForCampaign[i] != -1)
            {
                if (characters[selectedIndexCharacterForCampaign[i]].IsDead())
                {
                    selectedIndexCharacterForCampaign[i] = -1;
                }
            }
        }
    }

    public void AddCharacter(CharacterData character)
    {
        characters.Add(new CharacterContainer(character.id, new List<CharacterPassive>(), 1, true));
    }
}
