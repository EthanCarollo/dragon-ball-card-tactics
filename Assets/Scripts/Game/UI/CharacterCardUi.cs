using UnityEngine;

public class CharacterCardUi : MonoBehaviour
{
    public CharacterData[] characterData;
    public GameObject cardContainer;
    public GameObject cardPrefab;
    
    public void Start()
    {
        SetupCards();
    }

    public void SetupCards()
    {
        foreach (Transform child in cardContainer.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var character in characterData)
        {  
            GameObject card = Instantiate(cardPrefab, cardContainer.transform);
            card.GetComponent<CharacterCardBehaviour>().Setup(character);
        }
    }
}
