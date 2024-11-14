using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHealingUnderHealthPointPassive", menuName = "Passives/TransformationUnderHP")]
public class SuperSayanPassive : CharacterPassive
{
    private bool superSayanTransformation = false;
    public TransformAnimation transformAnimation;
    
    public override void Setup(BoardCharacter character)
    {
        superSayanTransformation = false;
    }

    public override void Update(BoardCharacter character)
    {
        base.Update(character);
        if (character.character.actualHealth < (character.character.GetCharacterData().maxHealth / 4) && superSayanTransformation == false)
        {
            Debug.Log("Successfully executed transformation passive");
            superSayanTransformation = true;
            character.Transform(transformAnimation);
            character.character.actualHealth += character.character.GetCharacterData().maxHealth / 4;
        }
    }
}