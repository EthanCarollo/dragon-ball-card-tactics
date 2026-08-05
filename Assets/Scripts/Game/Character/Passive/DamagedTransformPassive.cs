using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Transformation Passive", menuName = "Passives/TransformationOnHit")]
public class DamagedTransformPassive : TransformPassive
{
    public float hitThreshold = 200f; 

    public override void Setup(BoardCharacter character)
    {
        character.character.ResetPassiveRuntimeState(this);
    }

    public override void GetHit(int amount, BoardCharacter character)
    {
        var state = character.character.GetPassiveRuntimeState(this);
        if (amount >= hitThreshold && !state.triggered && transformAnimation.CanTransform(character))
        {
            character.PlayAnimation(transformAnimation);
            Debug.Log($"Transformation triggered when receive ${amount} damage on character, now he has ${character.character.actualHealth} hp.");
            state.triggered = true;
        }
        
    }
}
