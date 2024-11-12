using UnityEngine;
using TMPro;

public class SpecialAttackContainer : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    
    public void Setup(SpecialAttack attack)
    {
        nameText.text = attack.name;
        descriptionText.text = attack.description;
    }
}