using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveCard", menuName = "Card/PassiveCard")]
public class PassiveCard : UsableCharacterActionCard
{
    public CharacterPassive passive;

    public override string GetDescription()
    {
        var passiveName = passive == null ? "an unconfigured passive" : passive.passiveName;
        if(characterFor == null) return "Grants " + passiveName + " to a character";
        return "Grants " + passiveName + " to " + characterFor.characterName;
    }

    public override bool CanUseCard()
    {
        if(base.CanUseCard() == false){
            return false;
        }

        if (GameManager.Instance.GetCharactersOnBoard()
                .Count(cha => cha?.character != null && cha.character.isPlayerCharacter) == 0 || passive == null) return false;
        if (characterFor == null) return true;
        
        return GameManager.Instance.GetCharactersOnBoard()
                    .Where(cha => cha?.character != null && cha.character.isPlayerCharacter)
                    .Any(cha => cha.character.GetCharacterData() == characterFor ||
                               (cha.character.GetCharacterData()?.sameCharacters != null &&
                                cha.character.GetCharacterData().sameCharacters.Contains(characterFor)));
    }

    public override void UseCard()
    {
        LeanTween.delayedCall(0.5f, () =>
        {
            if(!CanUseCard()) {
                return;
            }
            var target = GetCharacterOnMouse();
            if (target != null && passive != null)
            {
                target.character.AddPassive(Instantiate(passive));
                GameManager.Instance.Player.Mana.CurrentMana -= manaCost;
                BoardGameUiManager.Instance?.ShowLooseMana(manaCost);
                BoardGameUiManager.Instance?.RefreshUI();
                RegisterCardHistory();
                GameManager.Instance.RemoveCard(this);
            }
        });
    }

    public override string GetCardType()
    {
        return "Passive";
    }
}
