using UnityEngine.EventSystems;
using UnityEngine;
using System.Linq;
using Coffee.UIEffects;
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
        if (CardDeckMenuUiManager.Instance == null || CardDatabase.Instance == null || blackOverlay == null)
        {
            return;
        }

        if(isInHand == true) {
            blackOverlay.SetActive(false);
            return;
        }
        if(CardDeckMenuUiManager.Instance.cardHandLimit <= (CardDatabase.Instance.playerCards?.Length ?? 0)){
            blackOverlay.SetActive(true);
        } else {
            blackOverlay.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (cardTypeContainer != null)
        {
            cardTypeContainer.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (cardTypeContainer != null)
        {
            cardTypeContainer.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Card == null || CardDeckMenuUiManager.Instance == null || CardDatabase.Instance == null)
        {
            return;
        }

        if(isInHand == false){
            if(CardDeckMenuUiManager.Instance.cardHandLimit <= (CardDatabase.Instance.playerCards?.Length ?? 0)) return;
            var cards = (CardDatabase.Instance.playerCards ?? new Card[0]).ToList();
            cards.Add(card);
            CardDatabase.Instance.playerCards = cards.ToArray();
            PlaySound(SoundDatabase.Instance?.addCardInDeckSound);
        } else {
            var cards = (CardDatabase.Instance.playerCards ?? new Card[0]).ToList();
            cards.Remove(card);
            CardDatabase.Instance.playerCards = cards.ToArray();
            PlaySound(SoundDatabase.Instance?.retireCardInDeckSound);
        }
        CardDeckMenuUiManager.Instance.RefreshUiCard();
    }

    public override void SetupCard(Card card){
        Card = card;
        if (cardTypeContainer != null)
        {
            cardTypeContainer.SetActive(false);
        }
        if (card == null)
        {
            base.SetupCard(null);
            return;
        }
        if (cardTypeText != null)
        {
            cardTypeText.text = card.GetCardType();
            cardTypeText.maskable = false;
        }
        base.SetupCard(card);
        if(effectForGui != null && card.uiEffectPreset != null && card.uiEffectPreset.Length != 0){
            effectForGui.LoadPreset(card.uiEffectPreset);
        }else if (effectForGui != null){
            effectForGui.LoadPreset("None");
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource == null || clip == null)
        {
            return;
        }

        audioSource.clip = clip;
        audioSource.Play();
    }
}
