using UnityEngine;
using UnityEngine.EventSystems;

public class ShowOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject hoverContainer;

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverContainer.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverContainer.SetActive(false);
    }
}