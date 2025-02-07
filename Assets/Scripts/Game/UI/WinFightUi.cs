using System;
using UnityEngine;

public class WinFightUi : MonoBehaviour
{
        public static WinFightUi Instance;
        public Transform winFightUi;

        public Card upgradeCard;
        public GetCardPrefab cardPrefabLeft;
        public GetCardPrefab cardPrefabMiddle;
        public GetCardPrefab cardPrefabRight;
        
        private void Awake()
        {
                Instance = this;
        }

        public void OpenWinFightUi(Board board)
        {
                winFightUi.gameObject.SetActive(true);
                var dropRate = new CardDropRate(GameManager.Instance.Player.Level.CurrentLevel);
                cardPrefabLeft.SetupCard(upgradeCard, false);
                cardPrefabMiddle.SetupCard(CardDatabase.Instance.GetRandomCard(dropRate.GetRarityOnDropRate()), true);
                cardPrefabRight.SetupCard(CardDatabase.Instance.GetRandomCard(dropRate.GetRarityOnDropRate()), true);
        }

        public void CloseWinFightUi()
        {
                winFightUi.gameObject.SetActive(false);
                GameManager.Instance.GoNextFight();
        }
}