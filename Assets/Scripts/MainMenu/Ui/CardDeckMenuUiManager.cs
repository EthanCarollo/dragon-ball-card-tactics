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
        Instance = this;
    }

    public void Start(){
        RefreshUiCard();
    }

    public void RefreshUiCard(){
        foreach (Transform child in cardDeckContainer)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in cardHandContainer)
        {
            Destroy(child.gameObject);
        }
        var playerCards = CardDatabase.Instance.playerCards.ToList();
        for (int i = 0; i < cardHandLimit; i++)
        {
            if(i > playerCards.Count-1){
                Instantiate(cardEmptyPrefab, cardHandContainer).GetComponentInChildren<TextMeshProUGUI>().text = (i+1).ToString();
                continue;
            }
            var card = playerCards[i];
            var cb = Instantiate(PrefabDatabase.Instance.cardDeckMainMenuPrefab, cardHandContainer)
                .GetComponent<CardDeckPrefab>();
            cb.SetupCard(card);
            cb.audioSource = audioSource;
            cb.isInHand = true;
            
        }
        
        var cards = CardDatabase.Instance.selectableCards.OrderBy(card => card.name).ToList();
        for (int i = 0; i < cards.Count; i++)
        {
            var card = cards[i];
            if(CardDatabase.Instance.playerCards.Contains(card)) continue;
            var cb = Instantiate(PrefabDatabase.Instance.cardDeckMainMenuPrefab, cardDeckContainer)
                .GetComponent<CardDeckPrefab>();
            cb.SetupCard(cards[i]);
            cb.audioSource = audioSource;
        }

    }

}