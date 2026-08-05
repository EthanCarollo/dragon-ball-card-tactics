using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stack Armor On Hit Passive", menuName = "Passives/StackArmorOnHitPassive")]
public class StackArmorOnHitPassive : CharacterPassive {
    public int armorStackedOnHit = 2;

    public override void Setup(BoardCharacter character)
    {
        character.character.ResetPassiveRuntimeState(this);
    }

    public override string GetDescription()
    {
        return "Add " + armorStackedOnHit + " armor on hit.";
    }

    public override void HitCharacter(BoardCharacter character, BoardCharacter target)
    {
        character.character.GetPassiveRuntimeState(this).stacks += armorStackedOnHit;
        character.character.NotifyCharacterChanged();
    }
    
    public override int AdditionalArmor(CharacterContainer character)
    {
        return character.GetPassiveRuntimeState(this).stacks;
    }
}
