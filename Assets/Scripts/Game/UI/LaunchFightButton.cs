using UnityEngine;
using UnityEngine.EventSystems;

public class LaunchFightButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        FightBoard.Instance.LaunchFight();
    }
}