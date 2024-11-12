using System;
using JetBrains.Annotations;
using UnityEngine;

[Serializable]
public class HealingUnderHealthPointPassive : CharacterPassive
{
    private bool healingLaunched = false;

    public override string GetName()
    {
        return "Healing";
    }
        
    public override string GetDescription()
    {
        return "Automatically restores health when HP falls below 25%";
    }
    
    public override void Setup(BoardCharacter character)
    {
        healingLaunched = false;
    }

    public override void Update(BoardCharacter character)
    {
        base.Update(character);
        if (character.character.actualHealth < (character.character.GetCharacterData().maxHealth / 4) && healingLaunched == false)
        {
            Debug.Log("Successfully executed HealingUnderHealthPointPassive");
            healingLaunched = true;
            character.character.actualHealth += character.character.GetCharacterData().maxHealth / 4;
        }
    }
}