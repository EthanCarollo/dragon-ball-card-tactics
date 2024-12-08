using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecialAttackContainer : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    public void Setup(SpecialAttack attack)
    {
        title.text = attack.name;
        description.text = attack.description;
    }
}