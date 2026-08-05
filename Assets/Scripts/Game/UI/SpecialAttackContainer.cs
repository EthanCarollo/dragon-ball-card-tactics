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
        if (specialAttack?.animation == null)
        {
            return;
        }

        if (description != null) description.text = specialAttack.animation.GetDetailledDescription(attackCharacter);
        RebuildLayout();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (specialAttack?.animation == null)
        {
            return;
        }

        if (description != null) description.text = specialAttack.animation.GetDescription(attackCharacter);
        RebuildLayout();
    }

    public void Setup(SpecialAttack attack, CharacterContainer character)
    {
        attackCharacter = character;
        specialAttack = attack;
        if (attack == null || attack.animation == null)
        {
            if (title != null) title.text = "No special attack";
            if (description != null) description.text = string.Empty;
            return;
        }

        if (title != null) title.text = attack.name;
        if (description != null) description.text = attack.animation.GetDescription(character);
        RebuildLayout();
        try
        {
            if(spriteImage != null) spriteImage.sprite = attack.animation.GetIcon();
        }
        catch (Exception error)
        {
            Debug.LogWarning(error);
        }
    }

    private void RebuildLayout()
    {
        var ownRectTransform = GetComponent<RectTransform>();
        if (ownRectTransform != null) LayoutRebuilder.ForceRebuildLayoutImmediate(ownRectTransform);

        var parentRectTransform = transform.parent?.GetComponent<RectTransform>();
        if (parentRectTransform != null) LayoutRebuilder.ForceRebuildLayoutImmediate(parentRectTransform);

        var descriptionParent = description?.transform.parent?.GetComponent<RectTransform>();
        if (descriptionParent != null) LayoutRebuilder.ForceRebuildLayoutImmediate(descriptionParent);
    }

    
}
