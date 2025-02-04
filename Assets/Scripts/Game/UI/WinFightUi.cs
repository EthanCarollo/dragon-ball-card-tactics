using System;
using UnityEngine;

public class WinFightUi : MonoBehaviour
{
        public static WinFightUi Instance;
        public Transform winFightUi;
        public CardPrefab[] cardPrefab;
        
        private void Awake()
        {
                Instance = this;
        }

        public void OpenWinFightUi(Board board)
        {
                winFightUi.gameObject.SetActive(true);
                var dropRate = new CardDropRate(GameManager.Instance.Player.Level.CurrentLevel);
                foreach (var card in cardPrefab)
                {
                        if(card is GetCardPrefab getCardPrefab){
                                getCardPrefab.isReloadable = true;
                        }
                        card.SetupCard(CardDatabase.Instance.GetRandomCard(dropRate.GetRarityOnDropRate()));
                }
        }

        public void CloseWinFightUi()
        {
                winFightUi.gameObject.SetActive(false);
                GameManager.Instance.GoNextFight();
        }
}