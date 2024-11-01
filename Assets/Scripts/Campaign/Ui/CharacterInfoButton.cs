using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoButton : MonoBehaviour
{
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterHp;
    public Slider healthSlider;
    public Image characterIcon;

    public void Setup(CharacterContainer characterData)
    {
        characterIcon.sprite = characterData.GetCharacterData().characterIcon;
        characterName.text = characterData.GetCharacterData().name;
        healthSlider.maxValue = characterData.GetCharacterData().maxHealth;
        healthSlider.value = characterData.actualHealth;
        characterHp.text = characterData.actualHealth + " / " + characterData.GetCharacterData().maxHealth + " HP";
    }
}