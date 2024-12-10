using System;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Card : ScriptableObject
{
    public int manaCost = 1;
    public CardRarity rarity = CardRarity.Common;
    public new string name;
    public Sprite image;

    public virtual string GetDescription()
    {
        return "";
    }

    public bool CanUseCard(){
        if(manaCost > GameManager.Instance.Player.Mana.CurrentMana){
            return false;
        }
        return true;
    }

    public Color GetRarityColor()
    {
        switch (rarity)
        {
            case CardRarity.Common:
                return Color.white;
            case CardRarity.Uncommon:
                return new Color(0f, 1f, 1f, 1f);
            case CardRarity.Rare:
                return new Color(1f, 1f, 0.5f, 1f);
            case CardRarity.Epic:
                return new Color(1f, 0.5f, 1f, 1f);
            default:
                return Color.white;
        }
    }
    
    public abstract void UseCard();
    public abstract void OnBeginDrag(PointerEventData eventData);
    public abstract void OnDrag(PointerEventData eventData);
    public abstract void OnEndDrag(PointerEventData eventData);
}

public enum CardRarity
{
    Common,
    Uncommon,
    Rare,
    Epic
}