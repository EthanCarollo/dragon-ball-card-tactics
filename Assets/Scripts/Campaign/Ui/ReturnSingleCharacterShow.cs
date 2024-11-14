using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;

public class ReturnSingleCharacterShow : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [NonSerialized]
    public CharacterContainer character;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (character != null)
        {
            SingleCharacterInfo.Instance.ShowCharacter(character);
        }
        Cursor.SetCursor(SpriteDatabase.Instance.normalCursor, Vector2.zero, CursorMode.Auto);
        this.gameObject.SetActive(false);
    }
    
    public Vector2 hotSpot = Vector2.zero; 
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(SpriteDatabase.Instance.pointerCursor, hotSpot, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(SpriteDatabase.Instance.normalCursor, Vector2.zero, CursorMode.Auto);
    }
}