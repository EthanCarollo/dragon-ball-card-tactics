using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sayan Spirit Passive", menuName = "Passives/SayanSpirit")]
public class SayanSpiritPassive : CharacterPassive
{
    public override int AdditionalAttack(CharacterContainer character)
    {
        float healthPercentage = (float)character.actualHealth / character.GetCharacterData().maxHealth * 100;
        if (character.actualHealth <= 0)
        {
            return 0;
        }
        float attackMultiplier = 75 / healthPercentage;
        return (int)(attackMultiplier * character.GetCharacterData().baseDamage);
    }
}