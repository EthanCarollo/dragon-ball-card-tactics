
using UnityEngine;

public class CharacterDeckUi : MonoBehaviour
{
    public static CharacterDeckUi Instance;
    public GameObject allCardsCharacter;
    public GameObject characterCardPrefab;
    public GameObject characterOneStarRow;
    public GameObject characterTwoStarRow;
    public GameObject characterThreeAndFourStarRow;
    public CharactersContainer charAllDataCloned;

    public void Awake()
    {
        Instance = this;
        charAllDataCloned = CharacterDatabase.Instance.characterDatas.Clone();
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

        for (int i = 0; i < charAllDataCloned.characters.Length; i++)
        {
            GameObject characterInstance = Instantiate(characterCardPrefab, allCardsCharacter.transform);
            CharacterDeckCardUi characterComponent = characterInstance.GetComponent<CharacterDeckCardUi>();
            if (characterComponent != null)
            {
                characterComponent.Setup(charAllDataCloned.characters[i], charAllDataCloned, this, i);
            }
        }
        
        for (int i = 0; i < GameManager.Instance.characterDeck.oneStarCharacters.characters.Length; i++)
        {
            GameObject characterInstance = Instantiate(characterCardPrefab, characterOneStarRow.transform);
            CharacterDeckCardUi characterComponent = characterInstance.GetComponent<CharacterDeckCardUi>();
            if (characterComponent != null)
            {
                characterComponent.Setup(GameManager.Instance.characterDeck.oneStarCharacters.characters[i], 
                    GameManager.Instance.characterDeck.oneStarCharacters, this, i);
            }
        }
        
        for (int i = 0; i < GameManager.Instance.characterDeck.twoStarCharacters.characters.Length; i++)
        {
            GameObject characterInstance = Instantiate(characterCardPrefab, characterTwoStarRow.transform);
            CharacterDeckCardUi characterComponent = characterInstance.GetComponent<CharacterDeckCardUi>();
            if (characterComponent != null)
            {
                characterComponent.Setup(GameManager.Instance.characterDeck.twoStarCharacters.characters[i], 
                    GameManager.Instance.characterDeck.twoStarCharacters, this, i);
            }
        }
        for (int i = 0; i < GameManager.Instance.characterDeck.threeStarCharacters.characters.Length; i++)
        {
            GameObject characterInstance = Instantiate(characterCardPrefab, characterThreeAndFourStarRow.transform);
            CharacterDeckCardUi characterComponent = characterInstance.GetComponent<CharacterDeckCardUi>();
            if (characterComponent != null)
            {
                characterComponent.Setup(GameManager.Instance.characterDeck.threeStarCharacters.characters[i], 
                    GameManager.Instance.characterDeck.threeStarCharacters, this, i);
            }
        }
        for (int i = 0; i < GameManager.Instance.characterDeck.fourStarCharacters.characters.Length; i++)
        {
            GameObject characterInstance = Instantiate(characterCardPrefab, characterThreeAndFourStarRow.transform);
            CharacterDeckCardUi characterComponent = characterInstance.GetComponent<CharacterDeckCardUi>();
            if (characterComponent != null)
            {
                characterComponent.Setup(GameManager.Instance.characterDeck.fourStarCharacters.characters[i], 
                    GameManager.Instance.characterDeck.fourStarCharacters, this, i);
            }
        }
    }
}
