using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDatabase", menuName = "Card/CardDatabase")]
public class CardDatabase : ScriptableObject
{
    private static CardDatabase _instance;

    public Card[] cards;

    public Card[] playerCards;

    public static CardDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<CardDatabase>("CardDatabase");
                if (_instance == null)
                {
                    Debug.LogError("CardDatabase instance not found in Resources folder!");
                }
            }
            return _instance;
        }
    }

    public Card GetRandomCard(CardRarity rarity)
    {
        // Filter cards by the specified rarity
        var filteredCards = cards.Where(card => card.rarity == rarity).ToArray();

        if (filteredCards.Length == 0)
        {
            Debug.LogWarning($"No cards found with rarity: {rarity}");
            return null;
        }

        // Get a random card from the filtered collection
        return filteredCards[Random.Range(0, filteredCards.Length)];
    }
        
}