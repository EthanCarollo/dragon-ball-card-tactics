using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardPrefab : MonoBehaviour, IDragHandler, IEndDragHandler {
    public Image characterImage;
    public TextMeshProUGUI characterName;
    private Card card;
    
    public void SetupCard(Card card){
        characterImage.sprite = card.image;
        characterName.text = card.name;
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