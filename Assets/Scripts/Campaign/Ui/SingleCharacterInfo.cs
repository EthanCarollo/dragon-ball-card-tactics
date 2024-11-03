using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleCharacterInfo : MonoBehaviour
{
    public static SingleCharacterInfo Instance;
    public TextMeshProUGUI characterName;
    public Image characterImage;
    public Slider healthSlider;
    public TextMeshProUGUI healthText;

    public void Awake()
    {
        Instance = this;
    }

    public void ShowCharacter(CharacterContainer character)
    {
        characterImage.sprite = character.GetCharacterData().characterSprite;
        characterName.text = character.GetCharacterData().name;
        healthText.text = character.actualHealth + " / " + character.GetCharacterData().maxHealth + " HP";
        healthSlider.maxValue = character.GetCharacterData().maxHealth;
        healthSlider.value = character.actualHealth;
    }
}
