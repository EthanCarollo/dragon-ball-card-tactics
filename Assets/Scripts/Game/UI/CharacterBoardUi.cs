using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBoardUi : MonoBehaviour
{
        public GameObject characterBoardUi;
        public TextMeshProUGUI charNameText;
        public Image charImage;

        public void ShowCharacterBoard(BoardCharacter character)
        {
                characterBoardUi.gameObject.SetActive(true);
                charNameText.text = character.character.name;
                charImage.sprite = character.character.characterSprite;
        }
}