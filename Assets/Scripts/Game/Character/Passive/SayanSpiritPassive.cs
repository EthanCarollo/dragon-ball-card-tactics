using System;

[Serializable]
public class SayanSpiritPassive : CharacterPassive
{
    public override string GetName()
    {
        return "Sayan Spirit";
    }
        
    public override string GetDescription()
    {
        return "Boosts attack as health decreases: 1.5x at 50% HP, 3x at 25% HP.";
    }
    
    public override int AdditionalAttack(CharacterContainer character)
    {
        float healthPercentage = (float)character.actualHealth / character.GetCharacterData().maxHealth * 100;
        float attackMultiplier = 75 / healthPercentage;
        return (int)(attackMultiplier * character.GetCharacterData().baseDamage);
    }
}