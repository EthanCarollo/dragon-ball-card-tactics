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

    public GameObject passivePrefabContainer;
    public GameObject passiveList;

    public GameObject specialAttackPrefabContainer;
    public GameObject specialAttackList;

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

        var tempIndex = 0;
        foreach (Transform child in specialAttackList.transform)
        {
            tempIndex++;
            if (tempIndex > 1)
            {
                Destroy(child.gameObject);
            }
        }
        
        if (character.GetCharacterData().specialAttackAnimation != null && character.GetCharacterData().specialAttackAnimation.GetLength(0) > 0)
        {
            foreach (var specialAttack in character.GetCharacterData().specialAttackAnimation)
            {
                Instantiate(specialAttackPrefabContainer, specialAttackList.transform).GetComponent<SpecialAttackContainer>().Setup(specialAttack);
            }
        }
        
        tempIndex = 0;
        foreach (Transform child in passiveList.transform)
        {
            tempIndex++;
            if (tempIndex > 1)
            {
                Destroy(child.gameObject);
            }
        }

        if (character.GetCharacterData().characterPassive != null && character.GetCharacterData().characterPassive.GetLength(0) > 0)
        {
            foreach (var passive in character.GetCharacterData().characterPassive)
            {
                Instantiate(passivePrefabContainer, passiveList.transform).GetComponent<PassiveContainer>().Setup(passive);
            }
        }
        
    }
}
