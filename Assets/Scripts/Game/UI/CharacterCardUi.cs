using TMPro;
using UnityEngine;

public class CharacterCardUi : MonoBehaviour
{
    public CharacterData[] characterData;
    public GameObject cardContainer;
    public GameObject cardPrefab;
    public TextMeshProUGUI manaText;
    
    public void Start()
    {
        SetupCards();
    }

    public void SetupCards()
    {
        manaText.text = GameManager.Instance.CurrentMana.ToString() + "/" + GameManager.Instance.MaxMana.ToString() + "C";
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
