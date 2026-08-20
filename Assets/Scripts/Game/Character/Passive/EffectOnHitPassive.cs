using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Effect On Hit Passive", menuName = "Passives/EffectOnHit")]
public class EffectOnHitPassive : CharacterPassive {
    public Effect effectApplied;

    public override void HitCharacter(BoardCharacter character, BoardCharacter target)
    {
        if(target?.character != null && effectApplied != null){
            target.character.AddEffect(effectApplied);
        }
    }
}
