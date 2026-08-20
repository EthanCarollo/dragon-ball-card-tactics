using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New IncreaseAttackWithSpeedPassive Passive", menuName = "Passives/IncreaseAttackWithSpeed")]
public class IncreaseAttackWithSpeedPassive : CharacterPassive {
    public override int AdditionalAttack(CharacterContainer character)
    {
        var characterData = character?.GetCharacterData();
        if (characterData == null)
        {
            return 0;
        }

        int speed = Mathf.Max(0, character.GetSpeed());
        float attackBonusMultiplier = 1f - (10f / (10f + speed));
        return Mathf.FloorToInt(characterData.baseDamage * attackBonusMultiplier);
    }
}
