using UnityEngine.EventSystems;
using UnityEngine;
using System.Linq;
using Coffee.UIEffects;
using Unity.VisualScripting;
using System;
using TMPro;

public class CardDeckPrefab : CardPrefab, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Card Card;
    public bool isInHand = false;
    public UIEffect effectForGui;
    public GameObject blackOverlay;
    public AudioSource audioSource;
    public TextMeshProUGUI cardTypeText;
    public GameObject cardTypeContainer;

    public void Update(){
        if(isInHand == true) {
            blackOverlay.SetActive(false);
            return;
        }
        if(CardDeckMenuUiManager.Instance.cardHandLimit <= CardDatabase.Instance.playerCards.Length){
            blackOverlay.SetActive(true);
        } else {
            blackOverlay.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cardTypeContainer.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cardTypeContainer.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isInHand == false){
            if(CardDeckMenuUiManager.Instance.cardHandLimit <= CardDatabase.Instance.playerCards.Length) return;
            var cards = CardDatabase.Instance.playerCards.ToList();
            cards.Add(card);
            CardDatabase.Instance.playerCards = cards.ToArray();
            audioSource.clip = SoundDatabase.Instance.addCardInDeckSound;
            audioSource.Play();
        } else {
            var cards = CardDatabase.Instance.playerCards.ToList();
            cards.Remove(card);
            CardDatabase.Instance.playerCards = cards.ToArray();
            audioSource.clip = SoundDatabase.Instance.retireCardInDeckSound;
            audioSource.Play();
        }
        CardDeckMenuUiManager.Instance.RefreshUiCard();
    }

    public override void SetupCard(Card card){
        Card = card;
        cardTypeContainer.SetActive(false);
        cardTypeText.text = card.GetCardType();
        cardTypeText.maskable = false;
        base.SetupCard(card);
        if(card.uiEffectPreset != null && card.uiEffectPreset.Length != 0){
            effectForGui.LoadPreset(card.uiEffectPreset);
        }else{
            effectForGui.LoadPreset("None");
        }
    }
}