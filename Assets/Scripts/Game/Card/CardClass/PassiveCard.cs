using System;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveCard", menuName = "Card/PassiveCard")]
public class PassiveCard : UsableCharacterActionCard
{
    public CharacterPassive passive;

    public override string GetDescription()
    {
        return "Grants " + passive.passiveName + " to " + characterFor.name;
    }

    public override void UseCard()
    {
        if(CanUseCard() == false) {
            return;
        }
        if (GetCharacterOnMouse() != null)
        {
            GetCharacterOnMouse().character.AddPassive(passive);
            GameManager.Instance.Player.Mana.CurrentMana -= manaCost;
            BoardGameUiManager.Instance.RefreshUI();
            GameManager.Instance.RemoveCard(this);
        }
    }
}