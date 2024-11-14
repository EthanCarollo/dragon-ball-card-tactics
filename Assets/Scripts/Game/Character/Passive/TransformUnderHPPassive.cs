using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Transformation Passive", menuName = "Passives/TransformationUnderHP")]
public class TransformUnderHPPassive : TransformPassive
{
    private bool launchTransformation = false;
    [Range(1, 100)]
    public float hpThresholdPercentage = 20f; 

    public override void Setup(BoardCharacter character)
    {
        launchTransformation = false;
    }

    public override void UpdatePassive(BoardCharacter character)
    {
        base.UpdatePassive(character);

        float hpThreshold = character.character.GetCharacterData().maxHealth * (hpThresholdPercentage / 100f);
        
        if (character.character.actualHealth < hpThreshold && !launchTransformation && transformAnimation.CanTransform(character))
        {
            FightBoard.Instance.LaunchTransformation(character, transformAnimation);
            Debug.Log($"Transformation triggered at {hpThresholdPercentage}% HP.");
            launchTransformation = true;
        }
    }
}
