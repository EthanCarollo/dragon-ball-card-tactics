using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Transformation Passive", menuName = "Passives/TransformationUnderHP")]
public class TransformPassive : CharacterPassive
{
    private bool superSayanTransformation = false;
    public TransformAnimation transformAnimation;
    [Range(1, 100)]
    public float hpThresholdPercentage = 20f; 

    public override void Setup(BoardCharacter character)
    {
        superSayanTransformation = false;
    }

    public override void UpdatePassive(BoardCharacter character)
    {
        base.UpdatePassive(character);

        float hpThreshold = character.character.GetCharacterData().maxHealth * (hpThresholdPercentage / 100f);
        
        if (character.character.actualHealth < hpThreshold && !superSayanTransformation)
        {
            FightBoard.Instance.LaunchTransformation(character, transformAnimation);
            Debug.Log($"Transformation triggered at {hpThresholdPercentage}% HP.");
            superSayanTransformation = true;
        }
    }
}
