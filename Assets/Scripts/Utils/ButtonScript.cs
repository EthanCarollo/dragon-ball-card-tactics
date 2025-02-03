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
        Cursor.SetCursor(SpriteDatabase.Instance.pointerCursor, hotSpot, CursorMode.Auto);
        audioSource.clip = SoundDatabase.Instance.hoverButtonSound;
        audioSource.Play();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        audioSource.clip = SoundDatabase.Instance.clickButtonSound;
        audioSource.Play();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(SpriteDatabase.Instance.normalCursor, Vector2.zero, CursorMode.Auto);
    }

}