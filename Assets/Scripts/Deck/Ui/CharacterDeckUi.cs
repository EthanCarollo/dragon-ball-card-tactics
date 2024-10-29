
using UnityEngine;

public class CharacterDeckUi : MonoBehaviour
{
    public static CharacterDeckUi Instance;
    public GameObject allCardsCharacter;
    public GameObject characterCardPrefab;
    public GameObject characterOneStarRow;
    public GameObject characterTwoStarRow;
    public GameObject characterThreeAndFourStarRow;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        SetupCharacterCards();
    }

    public void SetupCharacterCards()
    {
        foreach (Transform child in allCardsCharacter.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in characterOneStarRow.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in characterTwoStarRow.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in characterThreeAndFourStarRow.transform)
        {
            Destroy(child.gameObject);
        }
        
        for (int i = 0; i < CharacterDatabase.Instance.characterDatas.Length; i++)
        {
            GameObject characterInstance = Instantiate(characterCardPrefab, allCardsCharacter.transform);
            CharacterDeckCardUi characterComponent = characterInstance.GetComponent<CharacterDeckCardUi>();
            if (characterComponent != null && CharacterDatabase.Instance.characterDatas[i] != null)
            {
                characterComponent.Setup(CharacterDatabase.Instance.characterDatas[i]);
            }
        }
        
        for (int i = 0; i < GameManager.Instance.characterDeck.oneStarCharacters.Length; i++)
        {
            GameObject characterInstance = Instantiate(characterCardPrefab, characterOneStarRow.transform);
            CharacterDeckCardUi characterComponent = characterInstance.GetComponent<CharacterDeckCardUi>();
            if (characterComponent != null && GameManager.Instance.characterDeck.oneStarCharacters[i] != null)
            {
                characterComponent.Setup(GameManager.Instance.characterDeck.oneStarCharacters[i]);
            }
        }
        
        for (int i = 0; i < GameManager.Instance.characterDeck.twoStarCharacters.Length; i++)
        {
            GameObject characterInstance = Instantiate(characterCardPrefab, characterTwoStarRow.transform);
            CharacterDeckCardUi characterComponent = characterInstance.GetComponent<CharacterDeckCardUi>();
            if (characterComponent != null && GameManager.Instance.characterDeck.twoStarCharacters[i] != null)
            {
                characterComponent.Setup(GameManager.Instance.characterDeck.twoStarCharacters[i]);
            }
        }
        for (int i = 0; i < GameManager.Instance.characterDeck.threeStarCharacters.Length; i++)
        {
            GameObject characterInstance = Instantiate(characterCardPrefab, characterThreeAndFourStarRow.transform);
            CharacterDeckCardUi characterComponent = characterInstance.GetComponent<CharacterDeckCardUi>();
            if (characterComponent != null && GameManager.Instance.characterDeck.threeStarCharacters[i] != null)
            {
                characterComponent.Setup(GameManager.Instance.characterDeck.threeStarCharacters[i]);
            }
        }
        for (int i = 0; i < GameManager.Instance.characterDeck.fourStarCharacters.Length; i++)
        {
            GameObject characterInstance = Instantiate(characterCardPrefab, characterThreeAndFourStarRow.transform);
            CharacterDeckCardUi characterComponent = characterInstance.GetComponent<CharacterDeckCardUi>();
            if (characterComponent != null && GameManager.Instance.characterDeck.fourStarCharacters[i] != null)
            {
                characterComponent.Setup(GameManager.Instance.characterDeck.fourStarCharacters[i]);
            }
        }
    }
}
