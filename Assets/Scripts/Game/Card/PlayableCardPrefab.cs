using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayableCardPrefab : CardPrefab, IBeginDragHandler, IDragHandler, IEndDragHandler {
    public GameObject hideGameObject;

    public void Update(){
        SetCardColor();
    }

    public void SetCardColor(){
        hideGameObject.SetActive(!card.CanUseCard());
    }

    public void UseCard(){
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        card.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        card.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        card.OnEndDrag(eventData);
    }
}