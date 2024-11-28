using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardPrefab : MonoBehaviour {
    public Image characterImage;
    public TextMeshProUGUI characterName;

    public void SetupCard(CharacterContainer character){
        characterImage.sprite = character.GetCharacterData().characterSprite;
        characterName.name = character.GetCharacterData().name;
    }

    public void UseCard(){
        
    }
}