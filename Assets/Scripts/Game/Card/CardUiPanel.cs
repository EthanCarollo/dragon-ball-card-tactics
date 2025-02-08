using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardUiPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public HorizontalLayoutGroup horizontalLayout;

    public void OnPointerEnter(PointerEventData eventData)
    {
        LeanTween.cancel(this.gameObject);
        LeanTween.value(this.gameObject, f =>
        {
            horizontalLayout.spacing = f;
        }, horizontalLayout.spacing, 10f, 0.2f).setEaseInSine();
        LeanTween.value(this.gameObject, f =>
        {
            horizontalLayout.padding.top = Mathf.FloorToInt(f);
        }, horizontalLayout.padding.top, 0f, 0.2f).setEaseInSine();
        LeanTween.value(this.gameObject, f =>
        {
            horizontalLayout.padding.left = Mathf.FloorToInt(f);
        }, horizontalLayout.padding.left, 80f, 0.2f).setEaseInSine();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        LeanTween.cancel(this.gameObject);
        LeanTween.value(this.gameObject, f =>
        {
            horizontalLayout.spacing = f;
        }, horizontalLayout.spacing, -(130f + GameManager.Instance.PlayerCards.Count * 8), 0.2f).setEaseInSine();
        LeanTween.value(this.gameObject, f =>
        {
            horizontalLayout.padding.top = Mathf.FloorToInt(f);
        }, horizontalLayout.padding.top, 220f, 0.2f).setEaseInSine();
        LeanTween.value(this.gameObject, f =>
        {
            horizontalLayout.padding.left = Mathf.FloorToInt(f);
        }, horizontalLayout.padding.left, 40f, 0.2f).setEaseInSine();
    }
}