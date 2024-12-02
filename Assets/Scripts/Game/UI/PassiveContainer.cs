using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PassiveContainer : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Image passiveImage;
    private CharacterPassive passive;

    public void Setup(CharacterPassive passive)
    {
        this.passive = passive;
        if(nameText != null){
            nameText.text = passive.passiveName;
        }
        if(descriptionText != null){
            descriptionText.text = passive.passiveDescription;
        }
        if(passiveImage != null && passive.passiveImage != null){
            passiveImage.sprite = passive.passiveImage;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}