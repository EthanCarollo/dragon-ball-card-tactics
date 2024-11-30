using System;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public abstract class Card{
    public string name;
    public Sprite image;
    public abstract void UseCard();
    public abstract void OnDrag(PointerEventData eventData);
    public abstract void OnEndDrag(PointerEventData eventData);
}