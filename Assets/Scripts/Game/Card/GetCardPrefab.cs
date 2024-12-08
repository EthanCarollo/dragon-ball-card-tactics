using UnityEngine.EventSystems;
using UnityEngine;

public class GetCardPrefab : CardPrefab, IPointerClickHandler
{
        public void OnPointerClick(PointerEventData eventData)
        {
                Debug.Log("GetCardPrefab OnPointerClick");
                GameManager.Instance.AddCard(card);
                WinFightUi.Instance.CloseWinFightUi();
        }
}