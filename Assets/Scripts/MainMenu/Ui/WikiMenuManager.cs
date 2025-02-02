using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WikiMenuManager : MonoBehaviour {
    public int actualCharacter = 0;
    public Image actualCharacterImage;
    public CharacterBoardUi characterBoardInfoUi;
    public GameObject wayToObtainGameObject;
    public CardPreviewPrefab cardPreviewWayToObtain;

    public void Start(){
        actualCharacter = 0;
        wayToObtainGameObject.SetActive(false);
        RefreshUi();
    }

    public void GoNextCharacter(){
        actualCharacter++;
        if(actualCharacter >= CharacterDatabase.Instance.characterDatas.Length) actualCharacter = 0;
        RefreshUi();
    }

    public void GoPreviousCharacter(){
        actualCharacter--;
        if(actualCharacter < 0) actualCharacter = CharacterDatabase.Instance.characterDatas.Length-1;
        RefreshUi();
    }

    public void RefreshUi(){
        actualCharacterImage.sprite = CharacterDatabase.Instance.characterDatas[actualCharacter].characterSprite;
        characterBoardInfoUi.ShowCharacterBoard(new CharacterContainer(actualCharacter, 0, 0, 1, true));
        Card card = null;
        card = CardDatabase.Instance.cards.ToList().Find(listCard => {
            if(listCard is CharacterCard characterCard && characterCard.character.id == actualCharacter){
                return true;
            }
            if(listCard is TransformationCard transfoCard && transfoCard.transformations[0].transformation.newCharacterData.id == actualCharacter){
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