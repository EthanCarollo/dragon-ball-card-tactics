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
        wayToObtainGameObject.SetActive(false);
        RefreshUi();
    }

    public void GoNextCharacter(){
        actualCharacterIndex++;
        if(actualCharacterIndex >= CharacterDatabase.Instance.characterDatas.Length) actualCharacterIndex = 0;
        RefreshUi();
    }

    public void GoPreviousCharacter(){
        actualCharacterIndex--;
        if(actualCharacterIndex < 0) actualCharacterIndex = CharacterDatabase.Instance.characterDatas.Length-1;
        RefreshUi();
    }

    public void RefreshUi(){
        var charDatas = CharacterDatabase.Instance.characterDatas;
        charDatas = charDatas.OrderBy(character => character.characterName).ToArray();
        actualCharacterImage.sprite = charDatas[actualCharacterIndex].characterSprite;
        characterBoardInfoUi.ShowCharacterBoard(new CharacterContainer(charDatas[actualCharacterIndex].id, 0, 0, 1, true));
        Card card = null;
        card = CardDatabase.Instance.cards.ToList().Find(listCard => {
            if(listCard is CharacterCard characterCard && characterCard.character.id == charDatas[actualCharacterIndex].id){
                return true;
            }
            if(listCard is TransformationCard transfoCard && transfoCard.transformations[0].transformation.newCharacterData.id == charDatas[actualCharacterIndex].id){
                return true;
            }
            return false;
        });
        if(card != null){
            wayToObtainGameObject.SetActive(true);
            cardPreviewWayToObtain.SetupCard(card);
        }else{
            wayToObtainGameObject.SetActive(false);
        }
    }
}