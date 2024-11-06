using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SingleCharacterInfo : MonoBehaviour
{
    public static SingleCharacterInfo Instance;
    public GameObject characterContainer;
    public TextMeshProUGUI characterName;
    public Image characterImage;
    public Slider healthSlider;
    public TextMeshProUGUI typeText;
    public TextMeshProUGUI roleText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI maxHealthText;
    public TextMeshProUGUI maxKiText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI armorText;
    public TextMeshProUGUI rangeText;
    public TextMeshProUGUI speedText;
    public TextMeshProUGUI specialAttackText;

    public void Awake()
    {
        Instance = this;
        characterContainer.SetActive(false);
    }

    public void ShowCharacter(CharacterContainer character)
    {
        characterContainer.SetActive(true);
        characterImage.sprite = character.GetCharacterData().characterSprite;
        characterName.text = character.GetCharacterData().name;
        healthText.text = character.actualHealth + " / " + character.GetCharacterData().maxHealth + " HP";
        healthSlider.maxValue = character.GetCharacterData().maxHealth;
        typeText.text = "Type : " + character.GetCharacterData().characterType;
        roleText.text = "Role : " + character.GetCharacterData().role;
        healthSlider.value = character.actualHealth;
        maxHealthText.text = character.GetCharacterData().maxHealth + " MAX HP";
        maxKiText.text = character.GetCharacterData().maxKi + " MAX KI";
        damageText.text = character.GetCharacterData().baseDamage + " DMG";
        armorText.text = character.GetCharacterData().baseArmor + " AMR";
        rangeText.text = character.GetCharacterData().baseRange + " RNG";
        speedText.text = character.GetCharacterData().baseSpeed + " SPD";
        specialAttackText.text = character.GetCharacterSpecialAttack().name;
    }
}
