using TMPro;
using UnityEngine;

public class PassiveContainer : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    
    public void Setup(CharacterPassive passive)
    {
        nameText.text = passive.GetName();
        descriptionText.text = passive.GetDescription();
    }
}
