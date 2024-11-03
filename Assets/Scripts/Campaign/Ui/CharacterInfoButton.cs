using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterInfoButton : MonoBehaviour, IPointerClickHandler
{
    public CharacterContainer characterContainer;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterHp;
    public Slider healthSlider;
    public Image characterIcon;

    public void Setup(CharacterContainer characterContainer)
    {
        this.characterContainer = characterContainer;
        characterIcon.sprite = characterContainer.GetCharacterData().characterIcon;
        characterName.text = characterContainer.GetCharacterData().name;
        healthSlider.maxValue = characterContainer.GetCharacterData().maxHealth;
        healthSlider.value = characterContainer.actualHealth;
        characterHp.text = characterContainer.actualHealth + " / " + characterContainer.GetCharacterData().maxHealth + " HP";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SingleCharacterInfo.Instance.ShowCharacter(characterContainer);
    }
}