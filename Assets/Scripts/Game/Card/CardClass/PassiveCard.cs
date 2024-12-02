using System;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveCard", menuName = "Card/PassiveCard")]
public class PassiveCard : UsableActionCard
{
    public CharacterData characterFor;
    public CharacterPassive passive;
    public override void UseCard()
    {
        if (CanUseAction())
        {
            var boardObjects = GameManager.Instance.boardCharacterArray;
            foreach (var boardObject in boardObjects)
            {
                if (boardObject is BoardCharacter boardCharacter && boardCharacter.isPlayerCharacter && boardCharacter.character.GetCharacterData() == characterFor)
                {
                    var index = Array.IndexOf(boardCharacter.character.GetCharacterData().characterPassive, passive);
                    if (index != -1)
                    {
                        boardCharacter.character.unlockedPassives.Add(index);
                        Debug.Log(boardCharacter.character.unlockedPassives.ToString());
                    }
                
                }
            }
            GameManager.Instance.RemoveCard(this);
        }
    }

    protected override bool CanUseAction()
    {
        var boardObjects = GameManager.Instance.boardCharacterArray;
        foreach (var boardObject in boardObjects)
        {
            if (boardObject is BoardCharacter boardCharacter && boardCharacter.isPlayerCharacter && boardCharacter.character.GetCharacterData() == characterFor)
            {
                var index = Array.IndexOf(boardCharacter.character.GetCharacterData().characterPassive, passive);
                if (index != -1)
                {
                    return true;
                }
            }
        }
        return false;
    }
}