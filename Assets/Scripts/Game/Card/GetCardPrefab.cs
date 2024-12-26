using UnityEngine.EventSystems;
using UnityEngine;

public class GetCardPrefab : CardPrefab, IPointerClickHandler
{
        public bool isReloadable = false;
        public GameObject reloadButton;

        public void OnPointerClick(PointerEventData eventData)
        {
                Debug.Log("GetCardPrefab OnPointerClick");
                GameManager.Instance.AddCard(card);
                WinFightUi.Instance.CloseWinFightUi();
        }

        public override void SetupCard(Card card){
                if(isReloadable){
                        reloadButton.SetActive(true);
                }else{
                        reloadButton.SetActive(false);
                }
                base.SetupCard(card);
        }

        public void ReloadCard()
        {
                if(isReloadable == true){
                        var dropRate = new CardDropRate(GameManager.Instance.Player.Level.CurrentLevel);
                        isReloadable = false;
                        SetupCard(CardDatabase.Instance.GetRandomCard(dropRate.GetRarityOnDropRate()));
                }
        }
}