using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScript: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Vector2 hotSpot = Vector2.zero; 
    public AudioSource audioSource;

    void Start()
    {
        if(audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var spriteDatabase = SpriteDatabase.Instance;
        if (spriteDatabase != null && spriteDatabase.pointerCursor != null)
        {
            Cursor.SetCursor(spriteDatabase.pointerCursor, hotSpot, CursorMode.Auto);
        }

        PlaySound(SoundDatabase.Instance?.hoverButtonSound);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlaySound(SoundDatabase.Instance?.clickButtonSound);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        var spriteDatabase = SpriteDatabase.Instance;
        if (spriteDatabase != null && spriteDatabase.normalCursor != null)
        {
            Cursor.SetCursor(spriteDatabase.normalCursor, Vector2.zero, CursorMode.Auto);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource == null || clip == null)
        {
            return;
        }

        audioSource.PlayOneShot(clip);
    }

}
