using UnityEngine;
using UnityEngine.EventSystems;

public class CursorOnHoverUi : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector2 hotSpot = Vector2.zero; 

    public void OnPointerEnter(PointerEventData eventData)
    {
        var spriteDatabase = SpriteDatabase.Instance;
        if (spriteDatabase != null && spriteDatabase.pointerCursor != null)
        {
            Cursor.SetCursor(spriteDatabase.pointerCursor, hotSpot, CursorMode.Auto);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var spriteDatabase = SpriteDatabase.Instance;
        if (spriteDatabase != null && spriteDatabase.normalCursor != null)
        {
            Cursor.SetCursor(spriteDatabase.normalCursor, Vector2.zero, CursorMode.Auto);
        }
    }
    
}
