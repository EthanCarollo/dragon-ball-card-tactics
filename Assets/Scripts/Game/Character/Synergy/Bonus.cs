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

    public virtual void OnStartFight(bool isPlayerCharacter)
    {

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

    public override void OnStartFight(bool isPlayerCharacter)
    {
        var characters = GameManager.Instance.GetCharactersOnBoard().Where(character => character.isPlayerCharacter == isPlayerCharacter).ToList();
        var characterFound = characters.Find(character => character.character.GetCharacterData() == this.character);
        if(characterFound != null){
            characterFound.PlayAnimation(transform);
        } else {
            Debug.LogWarning("Didn't find character : " + character.characterName);
        }
    }
}