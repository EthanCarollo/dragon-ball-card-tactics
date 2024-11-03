using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterInventory", menuName = "Character/CharacterInventory")]
public class CharacterInventory : ScriptableObject
{
    public List<CharacterContainer> characters = new List<CharacterContainer>();

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
}

[Serializable]
public class CharacterContainer
{
    public int characterId;
    public int actualHealth;
    public int actualKi;
    
    public CharacterContainer(int characterId)
    {
        this.characterId = characterId;
        this.actualHealth = GetCharacterData().maxHealth;
    }

    public CharacterData GetCharacterData()
    {
        return CharacterDatabase.Instance.GetCharacterById(characterId);
    }
    
    public bool IsDead()
    {
        return actualHealth <= 0;
    }
    public int GetAttackDamage()
    {
        return GetCharacterData().baseDamage;
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
}
