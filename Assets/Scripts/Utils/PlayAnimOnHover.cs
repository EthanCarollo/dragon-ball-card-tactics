using Coffee.UIEffects;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayAnimOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        this.GetComponent<UIEffectTweener>().PlayForward();
    }    

    public void OnPointerExit(PointerEventData eventData)
    {
        this.GetComponent<UIEffectTweener>().Stop();
    }    
}
