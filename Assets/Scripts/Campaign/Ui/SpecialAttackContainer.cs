using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class SpecialAttackContainer : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Sprite spriteContainer;
    public Sprite highlightSpriteContainer;

    private int _attackIndex = 0;
    private SpecialAttack _specialAttack = null;
    private CharacterContainer _characterContainerHold;
    private SingleCharacterInfo _singleCharacterInfo;
    
    public void Setup(SpecialAttack attack, CharacterContainer characterContainer, int index, SingleCharacterInfo singleCharacterInfo)
    {
        _specialAttack = attack;
        _characterContainerHold = characterContainer;
        _attackIndex = index;
        _singleCharacterInfo = singleCharacterInfo;
        if (index == characterContainer.selectedUltimateAttack)
        {
            this.GetComponent<Image>().sprite = highlightSpriteContainer;
        }
        else
        {
            this.GetComponent<Image>().sprite = spriteContainer;
        }
        nameText.text = attack.name;
        descriptionText.text = attack.description;
    }
    
    public void Setup()
    {
        Setup(_specialAttack, _characterContainerHold, _attackIndex, _singleCharacterInfo);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _characterContainerHold.selectedUltimateAttack = _attackIndex;
        _singleCharacterInfo.UpdateAttack();
    }
}