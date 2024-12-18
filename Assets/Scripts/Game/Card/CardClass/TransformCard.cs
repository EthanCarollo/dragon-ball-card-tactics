using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TransformCard", menuName = "Card/TransformCard")]
public class TransformCard : UsableCharacterActionCard
{
    public TransformAnimation transform;

    public override string GetDescription()
    {
        return "Transform " + characterFor.characterName + " into " + transform.newCharacterData.characterName + ".";
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