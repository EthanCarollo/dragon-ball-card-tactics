using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveCard", menuName = "Card/PassiveCard")]
public class PassiveCard : UsableCharacterActionCard
{
    public CharacterPassive passive;

    public override string GetDescription()
    {
        if(characterFor == null) return "Grants " + passive.passiveName + " to a character";
        return "Grants " + passive.passiveName + " to " + characterFor.characterName;
    }

    public override bool CanUseCard()
    {
        if(base.CanUseCard() == false){
            return false;
        }

        if (GameManager.Instance.GetCharactersOnBoard()
                .Where(cha => cha.character.isPlayerCharacter).ToList().Count == 0) return false;
        if (characterFor == null) return true;
        
        return GameManager.Instance.GetCharactersOnBoard()
                    .Where(cha => cha.character.isPlayerCharacter).ToList()
                    .Find(cha => cha.character.GetCharacterData() == characterFor || cha.character.GetCharacterData().sameCharacters.Contains(characterFor)) != null;
    }

    public override void UseCard()
    {
        LeanTween.delayedCall(0.5f, () =>
        {
            if(CanUseCard() == false) {
                return;
            }
            if (GetCharacterOnMouse() != null)
            {
                GetCharacterOnMouse().character.AddPassive(passive);
                GameManager.Instance.Player.Mana.CurrentMana -= manaCost;
                BoardGameUiManager.Instance.ShowLooseMana(manaCost);
                BoardGameUiManager.Instance.RefreshUI();
                GameManager.Instance.RemoveCard(this);
            }
        });
    }
}