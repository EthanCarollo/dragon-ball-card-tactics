
using System.Collections.Generic;
using UnityEngine;

public class CardUi : MonoBehaviour {
    public static CardUi Instance;
    public GameObject cardPrefab;
    public CardUiPanel cardContainer;

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

    public void SetupCardUi(List<Card> cards){
        if (cardContainer == null || cardPrefab == null)
        {
            Debug.LogError("Cannot setup card UI: card container or card prefab is missing.");
            return;
        }

        foreach (Transform child in cardContainer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(Card card in cards ?? new List<Card>()){
            if (card == null)
            {
                continue;
            }

            GameObject cardObject = Instantiate(cardPrefab, cardContainer.transform);
            var cardPrefabScript = cardObject.GetComponent<PlayableCardPrefab>();
            if (cardPrefabScript == null)
            {
                Debug.LogError("Card prefab has no PlayableCardPrefab component.");
                Destroy(cardObject);
                continue;
            }

            cardPrefabScript.SetupCard(card);
        }
    }

    public void ShowCardUi(){
        cardContainer?.gameObject.SetActive(true);
    }

    public void HideCardUi(){
        cardContainer?.gameObject.SetActive(false);
    }
}
