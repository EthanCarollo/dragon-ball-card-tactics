using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveContainer : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Image passiveImage;
    
    public void Setup(CharacterPassive passive)
    {
        if(nameText != null){
            nameText.text = passive.GetName();
        }
        if(descriptionText != null){
            descriptionText.text = passive.GetDescription();
        }
        if(passiveImage != null && passive.passiveImage != null){
            passiveImage.sprite = passive.passiveImage;
        }
    }
}
