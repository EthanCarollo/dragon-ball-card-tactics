using UnityEngine.EventSystems;
using UnityEngine;
using System.Linq;
using Coffee.UIEffects;
using Unity.VisualScripting;

public class CardPreviewPrefab : CardPrefab
{
    public Card Card;
    public UIEffect effectForGui;
    public GameObject blackOverlay;

    public void Update(){
        if(CardDeckMenuUiManager.Instance.cardHandLimit <= CardDatabase.Instance.playerCards.Length){
            blackOverlay.SetActive(true);
        } else {
            blackOverlay.SetActive(false);
        }
    }

    public override void SetupCard(Card card){
        Card = card;
        base.SetupCard(card);
        Debug.Log(card.uiEffectPreset);
        if(card.uiEffectPreset != null && card.uiEffectPreset.Length != 0){
            Debug.Log(card.uiEffectPreset);
            effectForGui.LoadPreset(card.uiEffectPreset);
        }else{
            effectForGui.LoadPreset("None");
        }
    }
}