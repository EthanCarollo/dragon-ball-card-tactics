
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterDeckCardUi : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Image characterSprite;
    public Canvas parentCanvas;
    public static GameObject characterDraggedGameObject;

    public void Awake()
    {
        this.parentCanvas = GetComponentInParent<Canvas>();
    }

    public void Setup(CharacterData character)
    {
        this.characterSprite.sprite = character.characterIcon;
        this.characterSprite.enabled = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (characterDraggedGameObject == null)
        {
            characterDraggedGameObject = Instantiate(this.gameObject, parentCanvas.transform);
            characterDraggedGameObject.GetComponent<RectTransform>().sizeDelta =
                new Vector2(this.GetComponent<Image>().rectTransform.rect.width, this.GetComponent<Image>().rectTransform.rect.height);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (characterDraggedGameObject != null)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.transform as RectTransform,
                eventData.position,
                parentCanvas.worldCamera,
                out Vector2 localPoint);
            characterDraggedGameObject.GetComponent<RectTransform>().localPosition = localPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (characterDraggedGameObject != null)
        {
            Destroy(characterDraggedGameObject); 
            characterDraggedGameObject = null;
        }
    }
}
