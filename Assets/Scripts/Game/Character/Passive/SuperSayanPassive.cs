using System;
using UnityEngine;

[Serializable]
public class SuperSayanPassive : CharacterPassive
{
    private bool superSayanTransformation = false;
    public TransformAnimation transformAnimation;

    public override string GetName()
    {
        return "Super Sayan";
    }
        
    public override string GetDescription()
    {
        return "Automatically transform in Super Sayan when HP falls below 25%";
    }
    
    public override void Setup(BoardCharacter character)
    {
        superSayanTransformation = false;
    }

    public override void Update(BoardCharacter character)
    {
        base.Update(character);
        if (character.character.actualHealth < (character.character.GetCharacterData().maxHealth / 4) && superSayanTransformation == false)
        {
            Debug.Log("Successfully executed SuperSayanPassive transformation");
            superSayanTransformation = true;
            character.Transform(transformAnimation);
            character.character.actualHealth += character.character.GetCharacterData().maxHealth / 4;
        }
    }
}