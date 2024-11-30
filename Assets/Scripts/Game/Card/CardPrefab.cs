using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardPrefab : MonoBehaviour, IDragHandler, IEndDragHandler {
    public Image cardImage;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardMana;
    private Card card;
    
    public void SetupCard(Card card){
        cardImage.sprite = card.image;
        cardName.text = card.name;
        cardMana.text = card.manaCost.ToString();
        this.card = card;
    }

    public void UseCard(){
        
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