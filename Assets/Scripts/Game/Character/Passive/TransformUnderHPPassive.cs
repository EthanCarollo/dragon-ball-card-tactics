using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Transformation Passive", menuName = "Passives/TransformationUnderHP")]
public class TransformUnderHPPassive : TransformPassive
{
    [Range(1, 100)]
    public float hpThresholdPercentage = 20f; 

    public override void Setup(BoardCharacter character)
    {
        character.character.ResetPassiveRuntimeState(this);
    }

    public override void UpdatePassive(BoardCharacter character)
    {
        base.UpdatePassive(character);

        if (character?.character == null || transformAnimation == null)
        {
            return;
        }

        float hpThreshold = character.character.GetCharacterMaxHealth() * (Mathf.Clamp(hpThresholdPercentage, 0f, 100f) / 100f);
        var state = character.character.GetPassiveRuntimeState(this);
        
        if (state != null && character.character.actualHealth < hpThreshold && !state.triggered && transformAnimation.CanTransform(character))
        {
            character.PlayAnimation(transformAnimation);
            Debug.Log($"Transformation triggered at {hpThresholdPercentage}% HP.");
            state.triggered = true;
        }
    }
}
