using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class ButtonCanClickIfCardLimit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public GameObject cantLaunchInformation;

    public void Update(){
        var button = GetComponent<Button>();
        var cards = CardDatabase.Instance?.playerCards;
        var canLaunch = cards != null && cards.Any(card => card is CharacterCard);
        if (button != null) button.interactable = canLaunch;
        cantLaunchInformation?.SetActive(!canLaunch);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cantLaunchInformation?.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cantLaunchInformation?.SetActive(false);
    }
}
