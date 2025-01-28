using UnityEngine.EventSystems;
using UnityEngine;
using System.Linq;
using Coffee.UIEffects;

public class CardDeckPrefab : CardPrefab, IPointerClickHandler
{
    public Card Card;
    public bool isInHand = false;
    public UIEffect effectForGui;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isInHand == false){
            if(CardDeckMenuUiManager.Instance.cardHandLimit <= CardDatabase.Instance.playerCards.Length) return;
            var cards = CardDatabase.Instance.playerCards.ToList();
            cards.Add(card);
            CardDatabase.Instance.playerCards = cards.ToArray();
        } else {
            var cards = CardDatabase.Instance.playerCards.ToList();
            cards.Remove(card);
            CardDatabase.Instance.playerCards = cards.ToArray();
        }
        CardDeckMenuUiManager.Instance.RefreshUiCard();
    }

    public override void SetupCard(Card card){
        Card = card;
        base.SetupCard(card);
        if(card.highlight == true){
            effectForGui.LoadPreset("Shiny");
        }
    }
}