using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
                    return null;
                }

                _instance.AssignUniqueIDs();
            }
            return _instance;
        }
    }

    public void AssignUniqueIDs()
    {
        if (cards != null)
        {
            for (int i = 0; i < cards.Length; i++)
            {
                if (cards[i] != null)
                {
                    cards[i].id = i;
                }
            }
        }
    }

    public Card GetRandomCard(CardRarity rarity, IEnumerable<Card> excludedCards = null)
    {
        var excludedCardSet = excludedCards == null
            ? new HashSet<Card>()
            : new HashSet<Card>(excludedCards.Where(card => card != null));

        var availableCards = GetRewardCards(excludedCardSet).ToList();
        var filteredCards = availableCards.Where(card => card.rarity == rarity).ToList();

        if (filteredCards.Count == 0)
        {
            // Keep the requested rarity whenever possible. If it is exhausted by
            // the progression rules or the already displayed cards, fall back to
            // the highest available lower rarity.
            filteredCards = availableCards
                .Where(card => card.rarity <= rarity)
                .OrderByDescending(card => card.rarity)
                .ToList();
        }

        if (filteredCards.Count == 0)
        {
            Debug.LogWarning($"No cards found with rarity: {rarity}");
            return null;
        }

        return filteredCards[UnityEngine.Random.Range(0, filteredCards.Count)];
    }

    public bool IsTransformationCardProgressionAvailable(TransformationCard transformationCard)
    {
        if (transformationCard == null || transformationCard.transformations == null)
        {
            return false;
        }

        return GetPlayerCharacters().Any(character =>
            transformationCard.transformations.Any(transformation =>
                transformation != null &&
                transformation.character == character.character.GetCharacterData() &&
                CardRewardRules.IsTransformationProgressionAvailable(
                    transformationCard.manaCost,
                    GetMinimumTransformationCost(transformation.character))));
    }

    private IEnumerable<Card> GetRewardCards(ISet<Card> excludedCards)
    {
        if (cards == null)
        {
            return Enumerable.Empty<Card>();
        }

        int maximumManaCost = GetMaximumRewardManaCost();
        return cards.Where(card =>
            card != null &&
            !excludedCards.Contains(card) &&
            card.manaCost <= maximumManaCost &&
            IsRewardCardAvailable(card));
    }

    private bool IsRewardCardAvailable(Card card)
    {
        if (card is CharacterCard characterCard)
        {
            if (characterCard.character == null || IsCharacterAlreadyOwned(characterCard.character))
            {
                return false;
            }
        }

        if (card is TransformationCard transformationCard)
        {
            return IsTransformationCardProgressionAvailable(transformationCard);
        }

        return true;
    }

    private bool IsCharacterAlreadyOwned(CharacterData characterData)
    {
        bool isInPlayerCards = GameManager.Instance.PlayerCards.Any(card =>
            card is CharacterCard characterCard &&
            characterCard.character != null &&
            IsSameCharacterFamily(characterCard.character, characterData));

        if (isInPlayerCards)
        {
            return true;
        }

        return GetPlayerCharacters().Any(character =>
            IsSameCharacterFamily(character.character.GetCharacterData(), characterData));
    }

    private IEnumerable<BoardCharacter> GetPlayerCharacters()
    {
        return GameManager.Instance.GetCharactersOnBoard()
            .Where(character =>
                character != null &&
                character.character != null &&
                character.character.isPlayerCharacter &&
                !character.character.IsDead());
    }

    private static bool IsSameCharacterFamily(CharacterData first, CharacterData second)
    {
        if (first == null || second == null)
        {
            return false;
        }

        if (first == second)
        {
            return true;
        }

        return (first.sameCharacters != null && first.sameCharacters.Contains(second)) ||
               (second.sameCharacters != null && second.sameCharacters.Contains(first));
    }

    private int GetMinimumTransformationCost(CharacterData sourceCharacter)
    {
        if (cards == null || sourceCharacter == null)
        {
            return int.MaxValue;
        }

        return cards
            .OfType<TransformationCard>()
            .SelectMany(card => card.transformations ?? Array.Empty<TransformationsPossible>(),
                (card, transformation) => new { card, transformation })
            .Where(item =>
                item.transformation != null &&
                item.transformation.character == sourceCharacter &&
                item.transformation.transformation != null &&
                item.transformation.transformation.newCharacterData != null &&
                item.transformation.transformation.newCharacterData != sourceCharacter)
            .Select(item => item.card.manaCost)
            .DefaultIfEmpty(int.MaxValue)
            .Min();
    }

    private int GetMaximumRewardManaCost()
    {
        return CardRewardRules.GetMaximumRewardManaCost(GameManager.Instance.actualRound);
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
