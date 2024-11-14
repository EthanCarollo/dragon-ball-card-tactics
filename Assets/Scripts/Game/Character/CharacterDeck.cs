using System;
using System.Collections.Generic;
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

    public void AddCharacter(CharacterData character)
    {
        characters.Add(new CharacterContainer(character.id));
    }

    // By default, a Character Deck is only constitued of Goku & Piccolo
    [ContextMenu("Reset Value")]
    public void Reset()
    {
        characters = new List<CharacterContainer>()
        {
            new CharacterContainer(0),
            new CharacterContainer(1)
        };
    }
}

[Serializable]
public class CharacterContainer
{
    public int characterId;
    public int actualHealth;
    public int actualKi;
    public int selectedUltimateAttack = 0;
    
    public CharacterContainer(int characterId)
    {
        this.characterId = characterId;
        this.actualHealth = GetCharacterData().maxHealth;
    }
    
    public CharacterContainer(int characterId, int actualHealth, int acutalKi)
    {
        this.characterId = characterId;
        this.actualHealth = actualHealth;
        this.actualHealth = acutalKi;
    }
    
    public CharacterData GetCharacterData()
    {
        return CharacterDatabase.Instance.GetCharacterById(characterId);
    }

    public SpecialAttack GetCharacterSpecialAttack()
    {
        return CharacterDatabase.Instance.GetCharacterById(characterId).specialAttackAnimation[selectedUltimateAttack];
    }
    
    public bool IsDead()
    {
        return actualHealth <= 0;
    }
    public int GetCharacterMaxHealth()
    {
        return GetCharacterData().maxHealth;
    }
    public int GetAttackDamage()
    {
        int totalAdditionalAttack = 0;

        if(GetCharacterData().characterPassive != null)
        {
            foreach (var passive in GetCharacterData().characterPassive)
            {
                if(passive != null)
                {
                    totalAdditionalAttack += passive.AdditionalAttack(this);
                }
            }
        }
        
        return GetCharacterData().baseDamage + totalAdditionalAttack;
    }
    public int GetArmor()
    {
        return GetCharacterData().baseArmor;
    }
    public int GetSpeed()
    {
        return GetCharacterData().baseSpeed;
    }
    public float GetAttackSpeed()
    {
        return GetCharacterData().baseAttackSpeed;
    }
    public int GetCriticalChance()
    {
        return GetCharacterData().baseCriticalChance;
    }
    public int GetRange()
    {
        return GetCharacterData().baseRange;
    }
    public int GetCharacterPower()
    {
        return 
            Mathf.FloorToInt(GetAttackDamage() * 3) +
            Mathf.FloorToInt(GetArmor() * 2) +
            GetCriticalChance() +
            Mathf.FloorToInt(GetCharacterMaxHealth() / 25) +
            Mathf.FloorToInt(GetAttackSpeed() * 60);
    }
}
