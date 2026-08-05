using UnityEngine.EventSystems;
using UnityEngine;
using Coffee.UIEffects;

public class CardPreviewPrefab : CardPrefab
{
    public Card Card;
    public UIEffect effectForGui;

    public void Update(){

    }

    public override void SetupCard(Card card){
        Card = card;
        base.SetupCard(card);
        if (effectForGui != null)
        {
            if(card != null && card.uiEffectPreset != null && card.uiEffectPreset.Length != 0){
                effectForGui.LoadPreset(card.uiEffectPreset);
            }else{
                effectForGui.LoadPreset("None");
            }
        }
    }
}
