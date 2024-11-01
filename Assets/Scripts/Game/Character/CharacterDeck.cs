using System;
using System.Collections.Generic;

[Serializable]
public class CharacterInventory
{
    public List<CharacterContainer> characters = new List<CharacterContainer>();
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
