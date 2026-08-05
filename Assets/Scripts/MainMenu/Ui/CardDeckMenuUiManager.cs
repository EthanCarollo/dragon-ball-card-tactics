using System.Linq;
using TMPro;
using UnityEngine;

public class CardDeckMenuUiManager : MonoBehaviour {
    public Transform cardDeckContainer;
    public Transform cardHandContainer;
    public static CardDeckMenuUiManager Instance;
    public int cardHandLimit = 3;
    public GameObject cardEmptyPrefab;
    public AudioSource audioSource;

    public void Awake(){
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void Start(){
        RefreshUiCard();
    }

    public void RefreshUiCard(){
        if (cardDeckContainer == null || cardHandContainer == null)
        {
            Debug.LogError("Cannot refresh the card deck: one or more card containers are missing.");
            return;
        }

        var cardDatabase = CardDatabase.Instance;
        var prefabDatabase = PrefabDatabase.Instance;
        if (cardDatabase == null || prefabDatabase == null)
        {
            Debug.LogError("Cannot refresh the card deck: a required database is missing.");
            return;
        }

        ClearContainer(cardDeckContainer);
        ClearContainer(cardHandContainer);

        var playerCards = (cardDatabase.playerCards ?? new Card[0])
            .Where(card => card != null)
            .ToList();
        int handLimit = Mathf.Max(0, cardHandLimit);
        for (int i = 0; i < handLimit; i++)
        {
            if (i >= playerCards.Count)
            {
                if (cardEmptyPrefab == null)
                {
                    continue;
                }

                var emptyCard = Instantiate(cardEmptyPrefab, cardHandContainer);
                var emptyText = emptyCard.GetComponentInChildren<TextMeshProUGUI>();
                if (emptyText != null)
                {
                    emptyText.text = (i + 1).ToString();
                }

                continue;
            }

            var cardPrefab = CreateCardPrefab(prefabDatabase.cardDeckMainMenuPrefab, cardHandContainer, playerCards[i]);
            if (cardPrefab != null)
            {
                cardPrefab.audioSource = audioSource;
                cardPrefab.isInHand = true;
            }
        }

        var cards = (cardDatabase.selectableCards ?? new Card[0])
            .Where(card => card != null)
            .OrderBy(card => card.name)
            .ToList();
        foreach (var card in cards)
        {
            if (playerCards.Contains(card))
            {
                continue;
            }

            var cardPrefab = CreateCardPrefab(prefabDatabase.cardDeckMainMenuPrefab, cardDeckContainer, card);
            if (cardPrefab != null)
            {
                cardPrefab.audioSource = audioSource;
            }
        }

    }

    private static void ClearContainer(Transform container)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    }

    private static CardDeckPrefab CreateCardPrefab(GameObject prefab, Transform parent, Card card)
    {
        if (prefab == null || card == null)
        {
            Debug.LogWarning("Skipped a card because its prefab or data is missing.");
            return null;
        }

        var cardObject = Instantiate(prefab, parent);
        var cardPrefab = cardObject.GetComponent<CardDeckPrefab>();
        if (cardPrefab == null)
        {
            Debug.LogError($"Card prefab '{prefab.name}' has no CardDeckPrefab component.");
            Destroy(cardObject);
            return null;
        }

        cardPrefab.SetupCard(card);
        return cardPrefab;
    }

}
