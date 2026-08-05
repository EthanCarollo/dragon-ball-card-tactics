using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WikiMenuManager : MonoBehaviour {
    public int actualCharacterIndex = 0;
    public Image actualCharacterImage;
    public CharacterBoardUi characterBoardInfoUi;
    public GameObject wayToObtainGameObject;
    public CardPreviewPrefab cardPreviewWayToObtain;

    public void Start(){
        actualCharacterIndex = 0;
        wayToObtainGameObject?.SetActive(false);
        RefreshUi();
    }

    public void GoNextCharacter(){
        var characters = GetCharacters();
        if (characters.Length == 0)
        {
            return;
        }

        actualCharacterIndex++;
        if(actualCharacterIndex >= characters.Length) actualCharacterIndex = 0;
        RefreshUi();
    }

    public void GoPreviousCharacter(){
        var characters = GetCharacters();
        if (characters.Length == 0)
        {
            return;
        }

        actualCharacterIndex--;
        if(actualCharacterIndex < 0) actualCharacterIndex = characters.Length-1;
        RefreshUi();
    }

    public void RefreshUi(){
        var charDatas = GetCharacters();
        if (charDatas.Length == 0 || actualCharacterImage == null || CharacterDatabase.Instance == null)
        {
            wayToObtainGameObject?.SetActive(false);
            return;
        }

        actualCharacterIndex = Mathf.Clamp(actualCharacterIndex, 0, charDatas.Length - 1);
        var selectedCharacter = charDatas[actualCharacterIndex];
        actualCharacterImage.sprite = selectedCharacter.characterSprite;
        characterBoardInfoUi?.ShowCharacterBoard(new CharacterContainer(selectedCharacter.id, 0, 0, 1, true));
        Card card = null;
        var cards = CardDatabase.Instance?.cards ?? new Card[0];
        card = cards.Where(listCard => listCard != null).ToList().Find(listCard => {
            if(listCard is CharacterCard characterCard && characterCard.character == selectedCharacter){
                return true;
            }
            if(listCard is TransformationCard transfoCard &&
               (transfoCard.transformations ?? new TransformationsPossible[0]).Any(transformation =>
                   transformation?.transformation?.newCharacterData == selectedCharacter)){
                return true;
            }
            return false;
        });
        if(card != null){
            wayToObtainGameObject?.SetActive(true);
            cardPreviewWayToObtain?.SetupCard(card);
        }else{
            wayToObtainGameObject?.SetActive(false);
        }
    }

    private static CharacterData[] GetCharacters()
    {
        return (CharacterDatabase.Instance?.characterDatas ?? new CharacterData[0])
            .Where(character => character != null)
            .OrderBy(character => character.characterName)
            .ToArray();
    }
}
