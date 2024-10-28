using UnityEngine;
using UnityEngine.UI;

public class CharacterCardBehaviour : MonoBehaviour
{
    public Image cardImage;
    
    public void Setup(CharacterData characterData)
    {
        cardImage.sprite = characterData.characterSprite;
    }
}
