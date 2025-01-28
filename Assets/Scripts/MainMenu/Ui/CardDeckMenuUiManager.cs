using System.Linq;
using UnityEngine;

public class CardDeckMenuUiManager : MonoBehaviour {
    public Transform cardDeckContainer;


    public void Start(){
        foreach (Transform child in cardDeckContainer)
        {
            Destroy(child.gameObject);
        }
        var cards = CardDatabase.Instance.cards.OrderBy(card => card.name).ToList();
        ;
        for (int i = 0; i < cards.Count; i++)
        {
            Instantiate(PrefabDatabase.Instance.cardDeckMainMenuPrefab, cardDeckContainer)
                .GetComponent<CardDeckPrefab>().SetupCard(cards[i]);
        }
    }

}