
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterDeckCardUi : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public Image characterSprite;
    public Canvas parentCanvas;
    public CharactersContainer charactersContainer;
    public CharacterData characterData;
    public int characterIndexInContainer;
    public static GameObject characterDraggedGameObject;
    public CharacterDeckUi characterDeckUi;

    public void Awake()
    {
        this.parentCanvas = GetComponentInParent<Canvas>();
    }

    public void Setup(CharacterData character, CharactersContainer attachedContainer, CharacterDeckUi charDeckUi, int index)
    {
        this.characterDeckUi = charDeckUi;
        this.charactersContainer = attachedContainer;
        this.characterIndexInContainer = index;
        if (character == null) return;
        this.characterData = character;
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
        
        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raycastResults);

        foreach (var result in raycastResults)
        {
            if (result.gameObject.GetComponent<CharacterDeckCardUi>() != null)
            {
                var charTarget = result.gameObject.GetComponent<CharacterDeckCardUi>();
                charactersContainer.SwapCharacter(charTarget.charactersContainer, characterIndexInContainer, charTarget.characterIndexInContainer);
                charTarget.characterDeckUi.SetupCharacterCards();
                break;
            }
        }
    }
}
