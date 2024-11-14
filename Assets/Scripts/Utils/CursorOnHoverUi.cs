using UnityEngine;
using UnityEngine.EventSystems;

public class CursorOnHoverUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
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
