using System;

public class PassiveCard : UsableActionCard
{
    public CharacterData characterFor;
    public CharacterPassive passive;
    public override void UseCard()
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
                }
                
            }
        }
        GameManager.Instance.RemoveCard(this);
    }

    protected override bool CanUseAction()
    {
        return true;
    }

    public static Card GetTestPassiveCard()
    {
        var card = new PassiveCard();
        
        card.characterFor = CharacterDatabase.Instance.GetCharacterById(0);
        card.passive = CharacterDatabase.Instance.GetCharacterById(0).characterPassive[0];
        card.image = CharacterDatabase.Instance.GetCharacterById(0).characterSprite;
        card.name = CharacterDatabase.Instance.GetCharacterById(0).characterName + " Passive (Test)";
        
        return card;
    }
}