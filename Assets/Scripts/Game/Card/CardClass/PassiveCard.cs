using System;
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
            var index = Array.IndexOf(GetCharacterOnMouse().character.GetCharacterData().characterPassive, passive);
            if (index != -1)
            {
                GetCharacterOnMouse().character.unlockedPassives.Add(index);
                Debug.Log(GetCharacterOnMouse().character.unlockedPassives.ToString());
            }
            else
            {
                Debug.LogError("No passive selected in the character");
            }
            GameManager.Instance.Player.Mana.CurrentMana -= manaCost;
            BoardGameUiManager.Instance.RefreshSlider();
            GameManager.Instance.RemoveCard(this);
        }
    }
}