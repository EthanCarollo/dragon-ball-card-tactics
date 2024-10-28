using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCardBehaviour : MonoBehaviour
{
    private CharacterData character;
    public Image cardImage;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI costText;
    public bool hasBeenUsed = false;

    public void Setup(CharacterData characterData)
    {
        character = characterData;
        cardImage.sprite = characterData.characterIcon;
        nameText.text = characterData.characterName;
        costText.text = characterData.starNumber.ToString();
    }

    public void Clicked()
    {
        if(hasBeenUsed == false)
        {
            // TODO : update this logics cause it is actually stupid
            BoardUtils.AddCharacter(GameManager.Instance.boardUsableCharacterArray, new BoardCharacter(character, true));
            VerticalBoard.Instance.CreateBoard();
            hasBeenUsed = true;
            DisableCard();
        }
    }

    public void DisableCard()
    {
        cardImage.color = new Color(1, 1, 1, 0.5f);
    }
}
