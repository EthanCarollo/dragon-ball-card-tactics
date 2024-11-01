using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoButton : MonoBehaviour
{
    public TextMeshProUGUI characterName;
    public Image characterIcon;

    public void Setup(CharacterContainer characterData)
    {
        characterIcon.sprite = characterData.GetCharacterData().characterIcon;
        characterName.text = characterData.GetCharacterData().name;
    }
}