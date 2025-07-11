using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Transformation Passive", menuName = "Passives/TransformationOnHit")]
public class DamagedTransformPassive : TransformPassive
{
    private bool launchTransformation = false;
    public float hitThreshold = 200f; 

    public override void Setup(BoardCharacter character)
    {
        launchTransformation = false;
    }

    public override void GetHit(int amount, BoardCharacter character)
    {
        if (amount >= hitThreshold && !launchTransformation && transformAnimation.CanTransform(character))
        {
            character.PlayAnimation(transformAnimation);
            Debug.Log($"Transformation triggered when receive ${amount} damage on character, now he has ${character.character.actualHealth} hp.");
            launchTransformation = true;
        }
        
    }
}
