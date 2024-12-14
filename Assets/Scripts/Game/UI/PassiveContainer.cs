using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PassiveContainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Image passiveImage;
    public GameObject passiveInformation;
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
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        passiveInformation.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        passiveInformation.SetActive(false);
    }
}