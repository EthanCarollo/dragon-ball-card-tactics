using System.Collections.Generic;
using System.Linq;
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

        private readonly List<Card> displayedCards = new List<Card>();
        
        private void Awake()
        {
                Instance = this;
        }

        public void OpenWinFightUi(Board board)
        {
                winFightUi.gameObject.SetActive(true);
                var dropRate = new CardDropRate(GameManager.Instance.Player.Level.CurrentLevel);
                displayedCards.Clear();

                if (upgradeCard != null)
                {
                        displayedCards.Add(upgradeCard);
                }

                cardPrefabLeft.SetupCard(upgradeCard, false);
                var middleCard = CardDatabase.Instance.GetRandomCard(dropRate.GetRarityOnDropRate(), displayedCards);
                displayedCards.Add(middleCard);
                var rightCard = CardDatabase.Instance.GetRandomCard(dropRate.GetRarityOnDropRate(), displayedCards);
                displayedCards.Add(rightCard);

                cardPrefabMiddle.SetupCard(middleCard, true);
                cardPrefabRight.SetupCard(rightCard, true);
        }

        public Card RerollCard(Card currentCard)
        {
                int currentCardIndex = displayedCards.IndexOf(currentCard);
                if (currentCardIndex < 0)
                {
                        return currentCard;
                }

                var excludedCards = displayedCards
                        .Where(card => card != null && card != currentCard)
                        .ToList();
                var dropRate = new CardDropRate(GameManager.Instance.Player.Level.CurrentLevel);
                var replacement = CardDatabase.Instance.GetRandomCard(dropRate.GetRarityOnDropRate(), excludedCards);

                if (replacement == null)
                {
                        return currentCard;
                }

                displayedCards[currentCardIndex] = replacement;
                return replacement;
        }

        public void CloseWinFightUi()
        {
                winFightUi.gameObject.SetActive(false);
                GameManager.Instance.GoNextFight();
        }
}
