using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "TransformCard", menuName = "Card/TransformCard")]
public class TransformCard : UsableCharacterActionCard
{
    public TransformAnimation transform;

    public override string GetDescription()
    {
        return "Transform " + characterFor.characterName + " into " + transform.newCharacterData.characterName + ".";
    }

    public override bool CanUseCard()
    {
        var characterOnBoard = GameManager.Instance.GetCharactersOnBoard()
                    .Where(cha => cha.isPlayerCharacter).ToList().Find(cha => cha.character.GetCharacterData() == characterFor);   
        if(characterOnBoard == null){
            return false;
        }
        return base.CanUseCard();
    }

    public override void UseCard()
    {
        if(CanUseCard() == false) {
            return;
        }
        if (GetCharacterOnMouse() != null)
        {
            GetCharacterOnMouse().PlayAnimation(transform);
            GameManager.Instance.Player.Mana.CurrentMana -= manaCost;
            BoardGameUiManager.Instance.ShowLooseMana(manaCost);
            BoardGameUiManager.Instance.RefreshUI();
            GameManager.Instance.RemoveCard(this);
        }
    }
}