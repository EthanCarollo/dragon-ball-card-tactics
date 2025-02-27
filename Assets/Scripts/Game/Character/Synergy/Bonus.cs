using System;
using UnityEngine;
using System.Linq;

[Serializable]
public class Bonus
{
    public int attackBonus;
    public int maxHpBonus;
    public float attackSpeedBonus;
    public int criticalChanceBonus;
    public Effect[] effectsOnHit;

    public virtual bool OnStartSetupAction(bool isPlayerCharacter)
    {
        return false;
    }
}

[Serializable]
public class SpecialCharacterBonus : Bonus
{
    public CharacterData character;
}

[Serializable]
public class TransformCharacterBonus : Bonus
{
    public CharacterData character;
    public TransformAnimation transform;

    public override bool OnStartSetupAction(bool isPlayerCharacter)
    {
        var characters = GameManager.Instance.GetCharactersOnBoard().Where(character => character.character.isPlayerCharacter == isPlayerCharacter).ToList();
        var characterFound = characters.Find(character => character.character.GetCharacterData() == this.character);
        if(characterFound != null){
            characterFound.PlayAnimation(transform);
            return true;
        } 
        Debug.LogWarning("Didn't find character : " + character.characterName);
        return false;
    }
}