using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHealingUnderHealthPointPassive", menuName = "Passives/HealingUnderHP")]
public class HealingUnderHealthPointPassive : CharacterPassive
{
    public override void Setup(BoardCharacter character)
    {
        character.character.ResetPassiveRuntimeState(this);
    }

    public override void UpdatePassive(BoardCharacter character)
    {
        base.UpdatePassive(character);
        var state = character.character.GetPassiveRuntimeState(this);
        if (character.character.actualHealth < (character.character.GetCharacterData().maxHealth / 4) && state.triggered == false)
        {
            Debug.Log("Successfully executed HealingUnderHealthPointPassive");
            state.triggered = true;
            character.character.actualHealth += character.character.GetCharacterData().maxHealth / 4;
        }
    }
}
