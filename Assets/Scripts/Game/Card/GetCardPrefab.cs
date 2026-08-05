using UnityEngine.EventSystems;
using UnityEngine;

public class GetCardPrefab : CardPrefab, IPointerClickHandler
{
        public bool isReloadable = false;
        public GameObject reloadButton;

        public void OnPointerClick(PointerEventData eventData)
        {
                if (card == null)
                {
                        return;
                }

                if (GameManager.Instance != null)
                {
                        GameManager.Instance.AddCard(card);
                }
                WinFightUi.Instance?.CloseWinFightUi();
        }

        public void SetupCard(Card newCard, bool isReloadable)
        {
                this.isReloadable = isReloadable;
                SetupCard(newCard);
        }

        public override void SetupCard(Card card){
                reloadButton?.SetActive(isReloadable);
                base.SetupCard(card);
        }

        public void ReloadCard()
        {
                if (isReloadable == true && card != null)
                {
                        isReloadable = false;
                        var replacement = WinFightUi.Instance?.RerollCard(card);
                        if (replacement != null)
                        {
                                SetupCard(replacement);
                        }
                }
        }
}
