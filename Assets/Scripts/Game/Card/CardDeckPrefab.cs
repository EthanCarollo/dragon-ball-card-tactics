using UnityEngine.EventSystems;
using UnityEngine;

public class CardDeckPrefab : CardPrefab, IPointerClickHandler
{
        public void OnPointerClick(PointerEventData eventData)
        {

        }

        public override void SetupCard(Card card){
                base.SetupCard(card);
        }
}