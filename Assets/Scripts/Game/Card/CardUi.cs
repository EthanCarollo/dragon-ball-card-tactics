
using System.Collections.Generic;
using UnityEngine;

public class CardUi : MonoBehaviour {
    public static CardUi Instance;
    public GameObject cardPrefab;
    public CardUiPanel cardContainer;

    public void Awake(){
        Instance = this;
    }

    public void SetupCardUi(List<Card> cards){
        foreach (Transform child in cardContainer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(Card card in cards){
            GameObject cardObject = Instantiate(cardPrefab, cardContainer.transform);
            cardObject.GetComponent<CardPrefab>().SetupCard(card);
        }
    }
}