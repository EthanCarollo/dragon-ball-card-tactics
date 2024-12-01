public class PassiveCard : UsableActionCard
{
    public CharacterData characterFor;
    public CharacterPassive passive;
    public override void UseCard()
    {
        GameManager.Instance.RemoveCard(this);
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