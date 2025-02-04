using UnityEngine.EventSystems;
using UnityEngine;
using System.Linq;
using Coffee.UIEffects;
using Unity.VisualScripting;

public class CardPreviewPrefab : CardPrefab
{
    public Card Card;
    public UIEffect effectForGui;

    public void Update(){

    }

    public override void SetupCard(Card card){
        Card = card;
        base.SetupCard(card);
        if(card.uiEffectPreset != null && card.uiEffectPreset.Length != 0){
            effectForGui.LoadPreset(card.uiEffectPreset);
        }else{
            effectForGui.LoadPreset("None");
        }
    }
}