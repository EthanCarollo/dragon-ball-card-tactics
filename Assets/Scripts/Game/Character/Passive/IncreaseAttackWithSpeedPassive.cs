using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New IncreaseAttackWithSpeedPassive Passive", menuName = "Passives/IncreaseAttackWithSpeed")]
public class IncreaseAttackWithSpeedPassive : CharacterPassive {
    public override int AdditionalAttack(CharacterContainer character)
    {
        var attackMitigation = 1 - (10 / (10 + character.GetSpeed()));
        return character.GetCharacterData().baseDamage * attackMitigation;
    }
}