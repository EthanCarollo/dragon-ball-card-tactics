using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Regen On Kill Passive", menuName = "Passives/RegenOnKillPassive")]
public class RegenOnKillPassive : CharacterPassive {
    public int regen = 250;

    public override void KilledAnEnemy(BoardCharacter character, BoardCharacter target)
    {
        character.character.Heal(regen);
    }
}