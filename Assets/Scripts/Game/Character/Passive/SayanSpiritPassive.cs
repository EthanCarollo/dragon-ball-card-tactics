using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sayan Spirit Passive", menuName = "Passives/SayanSpirit")]
public class SayanSpiritPassive : CharacterPassive
{
    public override int AdditionalAttack(CharacterContainer character)
    {
        if (character == null || character.IsDead())
        {
            return 0;
        }

        var characterData = character.GetCharacterData();
        if (characterData == null)
        {
            return 0;
        }

        int maxHealth = Mathf.Max(1, character.GetCharacterMaxHealth());
        float healthPercentage = Mathf.Max(0.01f, (float)character.actualHealth / maxHealth * 100f);
        float attackMultiplier = 75f / healthPercentage;
        return Mathf.FloorToInt(attackMultiplier * characterData.baseDamage);
    }
}
