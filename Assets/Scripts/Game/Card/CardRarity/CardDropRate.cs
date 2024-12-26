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
            case 1 : 
                dropRates[(int)CardRarity.Common] = 80;
                dropRates[(int)CardRarity.Uncommon] = 20;
                dropRates[(int)CardRarity.Rare] = 0;
                dropRates[(int)CardRarity.Epic] = 0;
                break;
            case 2 : 
                dropRates[(int)CardRarity.Common] = 60;
                dropRates[(int)CardRarity.Uncommon] = 30;
                dropRates[(int)CardRarity.Rare] = 10;
                dropRates[(int)CardRarity.Epic] = 0;
                break;
            case 3 : 
                dropRates[(int)CardRarity.Common] = 40;
                dropRates[(int)CardRarity.Uncommon] = 35;
                dropRates[(int)CardRarity.Rare] = 20;
                dropRates[(int)CardRarity.Epic] = 5;
                break;
            case 4 : 
                dropRates[(int)CardRarity.Common] = 20;
                dropRates[(int)CardRarity.Uncommon] = 40;
                dropRates[(int)CardRarity.Rare] = 30;
                dropRates[(int)CardRarity.Epic] = 10;
                break;
            case 5 : 
                dropRates[(int)CardRarity.Common] = 20;
                dropRates[(int)CardRarity.Uncommon] = 40;
                dropRates[(int)CardRarity.Rare] = 40;
                dropRates[(int)CardRarity.Epic] = 20;
                break;
            case 6 : 
                dropRates[(int)CardRarity.Common] = 20;
                dropRates[(int)CardRarity.Uncommon] = 20;
                dropRates[(int)CardRarity.Rare] = 30;
                dropRates[(int)CardRarity.Epic] = 30;
                break;
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
