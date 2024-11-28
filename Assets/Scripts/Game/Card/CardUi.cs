
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
        foreach(Card card in cards){

        }
    }
}