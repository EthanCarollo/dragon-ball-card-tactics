using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpecialAttackContainer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private CharacterContainer attackCharacter;
    private SpecialAttack specialAttack;
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;
    public Image spriteImage;

    public void OnPointerEnter(PointerEventData eventData)
    {
        description.text = specialAttack.animation.GetDetailledDescription(attackCharacter);
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.transform.parent.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(description.transform.parent.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        description.text = specialAttack.animation.GetDescription(attackCharacter);
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.transform.parent.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(description.transform.parent.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());
    }

    public void Setup(SpecialAttack attack, CharacterContainer character)
    {
        attackCharacter = character;
        specialAttack = attack;
        title.text = attack.name;
        description.text = attack.animation.GetDescription(character);
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.GetComponent<RectTransform>());
        try
        {
            if(attack.animation.GetIcon() != null) spriteImage.sprite = attack.animation.GetIcon();
        }
        catch (Exception error)
        {
            Debug.LogWarning(error);
        }
    }

    
}