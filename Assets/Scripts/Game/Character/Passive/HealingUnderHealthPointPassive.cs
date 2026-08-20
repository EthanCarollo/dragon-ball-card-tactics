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
        if (character?.character == null)
        {
            return;
        }

        var state = character.character.GetPassiveRuntimeState(this);
        int healThreshold = character.character.GetCharacterMaxHealth() / 4;
        if (state != null && healThreshold > 0 && character.character.actualHealth < healThreshold && state.triggered == false)
        {
            Debug.Log("Successfully executed HealingUnderHealthPointPassive");
            state.triggered = true;
            character.Heal(healThreshold);
        }
    }
}
