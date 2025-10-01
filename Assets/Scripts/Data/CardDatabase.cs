using System.Linq;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

[CreateAssetMenu(fileName = "CardDatabase", menuName = "Card/CardDatabase")]
public class CardDatabase : ScriptableObject
{
    private static CardDatabase _instance;
    
    [Tooltip("The full list of all cards in the game. This is the master collection.")]
    public Card[] cards; 
    
    [Tooltip("Cards that can be chosen in specific selections (e.g., draft).")]
    public Card[] selectableCards; 
    
    [Tooltip("Cards that belong to the player (e.g., starting deck).")]
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
            _instance.AssignUniqueIDs();
            return _instance;
        }
    }

    public void AssignUniqueIDs()
    {
        if (cards != null)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i].id = i;
            }
        }
    }

    public Card GetRandomCard(CardRarity rarity)
    {
        Debug.Log(rarity);
        var filteredCards = cards.Where(card => card.rarity == rarity).ToArray();

        if (filteredCards.Length == 0)
        {
            Debug.LogWarning($"No cards found with rarity: {rarity}");
            return null;
        }

        // Get a random card from the filtered collection
        return filteredCards[Random.Range(0, filteredCards.Length)];
    }


#if UNITY_EDITOR
    [ContextMenu("Refresh Card List")]
    public void RefreshCards()
    {
        string[] guids = AssetDatabase.FindAssets("t:Card");
        cards = new Card[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            cards[i] = AssetDatabase.LoadAssetAtPath<Card>(path);
        }

        EditorUtility.SetDirty(this);
        Debug.Log("Card list refreshed!");
    }
#endif

}