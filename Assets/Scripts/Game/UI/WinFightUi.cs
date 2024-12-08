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
                foreach (var card in cardPrefab)
                {
                        card.SetupCard(CardDatabase.Instance.cards[0]);
                }
        }

        public void CloseWinFightUi()
        {
                winFightUi.gameObject.SetActive(false);
        }
}