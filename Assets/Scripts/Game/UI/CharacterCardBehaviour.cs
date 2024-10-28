using UnityEngine;
using UnityEngine.UI;

public class CharacterCardBehaviour : MonoBehaviour
{
    private CharacterData character;
    public Image cardImage;
    public bool hasBeenUsed = false;

    public void Setup(CharacterData characterData)
    {
        character = characterData;
        cardImage.sprite = characterData.characterSprite;
    }

    public void Clicked()
    {
        if(hasBeenUsed == false)
        {
            // TODO : update this logics cause it is actually stupid
            BoardUtils.AddCharacter(GameManager.Instance.boardUsableCharacterArray, new BoardCharacter(character, true));
            VerticalBoard.Instance.CreateBoard();
            hasBeenUsed = true;
        }
    }
}
