
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
        
        for (int i = 0; i < CharacterDatabase.Instance.characterDatas.characters.Length; i++)
        {
            GameObject characterInstance = Instantiate(characterCardPrefab, allCardsCharacter.transform);
            CharacterDeckCardUi characterComponent = characterInstance.GetComponent<CharacterDeckCardUi>();
            if (characterComponent != null && CharacterDatabase.Instance.characterDatas.characters[i] != null)
            {
                characterComponent.Setup(CharacterDatabase.Instance.characterDatas.characters[i]);
            }
        }
        
        for (int i = 0; i < GameManager.Instance.characterDeck.oneStarCharacters.characters.Length; i++)
        {
            GameObject characterInstance = Instantiate(characterCardPrefab, characterOneStarRow.transform);
            CharacterDeckCardUi characterComponent = characterInstance.GetComponent<CharacterDeckCardUi>();
            if (characterComponent != null && GameManager.Instance.characterDeck.oneStarCharacters.characters[i] != null)
            {
                characterComponent.Setup(GameManager.Instance.characterDeck.oneStarCharacters.characters[i]);
            }
        }
        
        for (int i = 0; i < GameManager.Instance.characterDeck.twoStarCharacters.characters.Length; i++)
        {
            GameObject characterInstance = Instantiate(characterCardPrefab, characterTwoStarRow.transform);
            CharacterDeckCardUi characterComponent = characterInstance.GetComponent<CharacterDeckCardUi>();
            if (characterComponent != null && GameManager.Instance.characterDeck.twoStarCharacters.characters[i] != null)
            {
                characterComponent.Setup(GameManager.Instance.characterDeck.twoStarCharacters.characters[i]);
            }
        }
        for (int i = 0; i < GameManager.Instance.characterDeck.threeStarCharacters.characters.Length; i++)
        {
            GameObject characterInstance = Instantiate(characterCardPrefab, characterThreeAndFourStarRow.transform);
            CharacterDeckCardUi characterComponent = characterInstance.GetComponent<CharacterDeckCardUi>();
            if (characterComponent != null && GameManager.Instance.characterDeck.threeStarCharacters.characters[i] != null)
            {
                characterComponent.Setup(GameManager.Instance.characterDeck.threeStarCharacters.characters[i]);
            }
        }
        for (int i = 0; i < GameManager.Instance.characterDeck.fourStarCharacters.characters.Length; i++)
        {
            GameObject characterInstance = Instantiate(characterCardPrefab, characterThreeAndFourStarRow.transform);
            CharacterDeckCardUi characterComponent = characterInstance.GetComponent<CharacterDeckCardUi>();
            if (characterComponent != null && GameManager.Instance.characterDeck.fourStarCharacters.characters[i] != null)
            {
                characterComponent.Setup(GameManager.Instance.characterDeck.fourStarCharacters.characters[i]);
            }
        }
    }
}
