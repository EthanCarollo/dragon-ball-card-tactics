using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterInfoButton : MonoBehaviour, IPointerClickHandler
{
    public CharacterContainer characterContainer;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI characterPower;
    public TextMeshProUGUI characterHp;
    public Image characterContainerInner;
    public Image characterContainerOuter;
    public Slider healthSlider;
    public Image characterIcon;

    public void Setup(CharacterContainer characterContainer)
    {
        this.characterContainer = characterContainer;
        characterIcon.sprite = characterContainer.GetCharacterData().characterIcon;
        characterName.text = characterContainer.GetCharacterData().name;
        characterPower.text = "POWER : " + characterContainer.GetCharacterPower();
        healthSlider.maxValue = characterContainer.GetCharacterData().maxHealth;
        healthSlider.value = characterContainer.actualHealth;
        characterHp.text = characterContainer.actualHealth + " / " + characterContainer.GetCharacterData().maxHealth + " HP";

        characterContainerInner.color = characterContainer.GetCharacterData().GetCharacterColor();
        characterContainerOuter.color = characterContainer.GetCharacterData().GetCharacterColor();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SingleCharacterInfo.Instance.ShowCharacter(characterContainer);
    }
}