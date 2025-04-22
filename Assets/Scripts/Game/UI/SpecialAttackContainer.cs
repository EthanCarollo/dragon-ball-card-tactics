using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecialAttackContainer : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Image spriteImage;

    public void Setup(SpecialAttack attack)
    {
        title.text = attack.name;
        description.text = attack.description;
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());
        try
        {
            if(attack.animation.animationIcon != null) spriteImage.sprite = attack.animation.animationIcon;
        }
        catch (Exception error)
        {
            Debug.LogWarning(error);
        }
    }
}