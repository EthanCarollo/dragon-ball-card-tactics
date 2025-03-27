using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stack Armor On Hit Passive", menuName = "Passives/StackArmorOnHitPassive")]
public class StackArmorOnHitPassive : CharacterPassive {
    public int moreArmor = 0;
    public int armorStackedOnHit = 2;

    public override void Setup(BoardCharacter character)
    {
        moreArmor = 0;
    }

    public override void HitCharacter(BoardCharacter character, BoardCharacter target)
    {
        moreArmor += armorStackedOnHit;
    }
    
    public override int AdditionalArmor(CharacterContainer character)
    {
        return moreArmor;
    }
}