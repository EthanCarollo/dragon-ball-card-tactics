using System;

[Serializable]
public class Bonus
{
    public int attackBonus;
    public int maxHpBonus;
    public float attackSpeedBonus;
    public int criticalChanceBonus;
}

[Serializable]
public class SpecialCharacterBonus : Bonus
{
    public CharacterData character;
}