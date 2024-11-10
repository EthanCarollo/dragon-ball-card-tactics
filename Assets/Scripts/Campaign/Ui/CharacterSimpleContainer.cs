using UnityEngine;
using UnityEngine.UI;

public class CharacterSimpleContainer : MonoBehaviour {
    public Image backgroundCharacter;
    public Image characterIcon;

    public void SetupCharacter(CharacterData character){
        backgroundCharacter.color = character.GetCharacterColor();
        characterIcon.sprite = character.characterIcon;
    }
}