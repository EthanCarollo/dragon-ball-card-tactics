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
    public float powerMultiplicator = 1;
    public int characterId;
    public int actualHealth;
    public int actualKi;
    public int selectedUltimateAttack = 0;
    public List<int> unlockedPassives = new List<int>();
    
    public CharacterContainer(int characterId, float powerMultiplicator = 1)
    {
        this.powerMultiplicator = powerMultiplicator;
        this.characterId = characterId;
        this.actualHealth = GetCharacterMaxHealth();
    }
    
    public CharacterContainer(int characterId, int actualHealth, int actualKi)
    {
        this.characterId = characterId;
        this.actualHealth = actualHealth;
        this.actualKi = actualKi;
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

    public CharacterPassive[] GetCharacterPassives()
    {
        List<CharacterPassive> passives = new List<CharacterPassive>();
        foreach (int variablePassive in unlockedPassives)
        {
            if (variablePassive >= GetCharacterData().characterPassive.Length)
            {
                continue;
            }
            passives.Add(GetCharacterData().characterPassive[variablePassive]);
        }
        return passives.ToArray();
    }

    public int GetAttackDamage()
    {
        int totalAdditionalAttack = 0;

        if(GetCharacterPassives() != null)
        {
            foreach (var passive in GetCharacterPassives())
            {
                if(passive != null)
                {
                    totalAdditionalAttack += passive.AdditionalAttack(this);
                }
            }
        }

        // Synergy bonus part
        var synergies = GetSynergies();
        if(synergies != null && synergies.Count() > 0){
            foreach (var synergie in synergies)
            {
                foreach (var tierBonus in synergie.GetActiveTierBonuses())
                {
                    foreach (var bonus in tierBonus.Bonuses)
                    {
                        totalAdditionalAttack += bonus.attackBonus;   
                    }
                }
            }
        }
        
        
        return Mathf.FloorToInt((GetCharacterData().baseDamage + totalAdditionalAttack)  * powerMultiplicator);
    }

    public string GetName()
    {
        return GetCharacterData().name;
    }
    public int GetArmor()
    {
        return Mathf.FloorToInt(GetCharacterData().baseArmor * powerMultiplicator);
    }
    public int GetSpeed()
    {
        return GetCharacterData().baseSpeed;
    }
    public int GetCharacterMaxHealth()
    {
        int maxHealth = Mathf.FloorToInt(GetCharacterData().maxHealth * powerMultiplicator);

        var synergies = GetSynergies();
        if(synergies != null && synergies.Count() > 0){
            foreach (var synergie in synergies)
            {
                foreach (var tierBonus in synergie.GetActiveTierBonuses())
                {
                    foreach (var bonus in tierBonus.Bonuses)
                    {
                        maxHealth += bonus.maxHpBonus;   
                    }
                }
            }
        }
        
        return maxHealth;
    }
    public int GetCharacterMaxKi()
    {
        return GetCharacterData().maxKi;
    }
    public float GetAttackSpeed()
    {
        var attackSpeed = GetCharacterData().baseAttackSpeed  * powerMultiplicator;
        var synergies = GetSynergies();
        if(synergies != null && synergies.Count() > 0){
            foreach (var synergie in synergies)
            {
                foreach (var tierBonus in synergie.GetActiveTierBonuses())
                {
                    foreach (var bonus in tierBonus.Bonuses)
                    {
                        attackSpeed += bonus.attackSpeedBonus;   
                    }
                }
            }
        }
        return attackSpeed;
    }
    public int GetCriticalChance()
    {
        var criticalChance = GetCharacterData().baseCriticalChance;
        var synergies = GetSynergies();
        if(synergies != null && synergies.Count() > 0){
            foreach (var synergie in synergies)
            {
                foreach (var tierBonus in synergie.GetActiveTierBonuses())
                {
                    foreach (var bonus in tierBonus.Bonuses)
                    {
                        criticalChance += bonus.criticalChanceBonus;   
                    }
                }
            }
        }
        return criticalChance;
    }
    public int GetRange()
    {
        return GetCharacterData().baseRange;
    }
    public Synergy[] GetSynergies()
    {
        return GetCharacterData().synergies;
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
