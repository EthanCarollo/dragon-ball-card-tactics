using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PassiveContainer : MonoBehaviour, IPointerClickHandler
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Image passiveImage;
    private CharacterPassive passive;
    private bool canBeClicked = true;

    public void Setup(CharacterPassive passive)
    {
        this.passive = passive;
        if(nameText != null){
            nameText.text = passive.passiveName;
        } else {
            canBeClicked = false;
        }
        if(descriptionText != null){
            descriptionText.text = passive.passiveDescription;
        } else {
            canBeClicked = false;
        }
        if(passiveImage != null && passive.passiveImage != null){
            passiveImage.sprite = passive.passiveImage;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(passive is TransformPassive transformPassive && canBeClicked){
            SingleCharacterInfo.Instance.ShowCharacter(transformPassive.transformAnimation.newCharacterData);
        }
    }
}
