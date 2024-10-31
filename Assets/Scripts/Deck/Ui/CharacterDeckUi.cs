
using UnityEngine;

public class CharacterDeckUi : MonoBehaviour
{
    public static CharacterDeckUi Instance;
    public GameObject allCardsCharacter;
    public GameObject characterCardPrefab;
    public GameObject characterCardGridRow;
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
        foreach (Transform child in characterCardGridRow.transform)
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
        
        for (int i = 0; i < GameManager.Instance.characterDeck.characters.characters.Length; i++)
        {
            GameObject characterInstance = Instantiate(characterCardPrefab, characterCardGridRow.transform);
            CharacterDeckCardUi characterComponent = characterInstance.GetComponent<CharacterDeckCardUi>();
            if (characterComponent != null)
            {
                characterComponent.Setup(GameManager.Instance.characterDeck.characters.characters[i], 
                    GameManager.Instance.characterDeck.characters, this, i);
            }
        }
    }
}
