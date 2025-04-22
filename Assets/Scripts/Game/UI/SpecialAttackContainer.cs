using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpecialAttackContainer : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Image spriteImage;

    public void Setup(SpecialAttack attack, CharacterContainer character)
    {
        title.text = attack.name;
        description.text = attack.animation.GetDescription(character);
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