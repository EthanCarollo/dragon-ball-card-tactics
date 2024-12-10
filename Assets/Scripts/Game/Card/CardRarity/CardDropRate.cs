using UnityEngine;
using System.Linq;
using System;

public class CardDropRate
{
    private readonly int[] dropRates;

    public CardDropRate(int currentLevel)
    {
        dropRates = new int[4]; // Corresponds to Common, Uncommon, Rare, Epic

        switch (currentLevel)
        {
            default:
                Debug.Log($"Current level isn't taken into account, setting basic drop rate. Level: {currentLevel}");
                dropRates[(int)CardRarity.Common] = 100;
                dropRates[(int)CardRarity.Uncommon] = 0;
                dropRates[(int)CardRarity.Rare] = 0;
                dropRates[(int)CardRarity.Epic] = 0;
                break;
        }
    }

    public CardRarity GetRarityOnDropRate()
    {
        int total = dropRates.Sum();
        if (total == 0) throw new InvalidOperationException("Total drop rate cannot be zero.");

        int randomValue = UnityEngine.Random.Range(1, total + 1);
        int cumulative = 0;

        for (int i = 0; i < dropRates.Length; i++)
        {
            cumulative += dropRates[i];
            if (randomValue <= cumulative)
            {
                return (CardRarity)i;
            }
        }

        throw new Exception("Failed to determine rarity. This should never happen.");
    }

    public float GetRarityPercentage(CardRarity rarity)
    {
        int total = dropRates.Sum();
        if (total == 0) throw new InvalidOperationException("Total drop rate cannot be zero.");

        return (float)dropRates[(int)rarity] / total * 100;
    }
}
