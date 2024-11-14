using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PassiveContainer : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Image passiveImage;
    private CharacterPassive passive;
    private bool canBeClicked = true;

    public void Setup(CharacterPassive passive)
    {
        this.passive = passive;
        if(nameText != null){
            nameText.text = passive.passiveName;
        } else {
            canBeClicked = false;
        }
        if(descriptionText != null){
            descriptionText.text = passive.passiveDescription;
        } else {
            canBeClicked = false;
        }
        if(passiveImage != null && passive.passiveImage != null){
            passiveImage.sprite = passive.passiveImage;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(passive is TransformPassive transformPassive && canBeClicked){
            SingleCharacterInfo.Instance.ShowCharacter(transformPassive.transformAnimation.newCharacterData);
        }
    }
    
    public Vector2 hotSpot = Vector2.zero; 
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(passive is TransformPassive && canBeClicked) Cursor.SetCursor(SpriteDatabase.Instance.pointerCursor, hotSpot, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(passive is TransformPassive && canBeClicked) Cursor.SetCursor(SpriteDatabase.Instance.normalCursor, Vector2.zero, CursorMode.Auto);
    }
}
