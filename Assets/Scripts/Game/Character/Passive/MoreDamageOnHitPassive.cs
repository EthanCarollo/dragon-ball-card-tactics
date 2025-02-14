using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New More Damage On Hit Passive", menuName = "Passives/MoreDamageOnHitPassive")]
public class MoreDamageOnHitPassive : CharacterPassive {
    public int moreDamage = 20;
    
    public override void HitCharacter(BoardCharacter character, BoardCharacter target)
    {
        target.HitDamage(moreDamage);
    }
}