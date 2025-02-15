using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class ButtonCanClickIfCardLimit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public GameObject cantLaunchInformation;

    public void Update(){
        if(CardDatabase.Instance.playerCards.Length > 0 && CardDatabase.Instance.playerCards.Any(card => card is CharacterCard)){
            cantLaunchInformation.SetActive(false);
            GetComponent<Button>().interactable = true;
        }else{
            GetComponent<Button>().interactable = false;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        cantLaunchInformation.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        cantLaunchInformation.SetActive(false);
    }
}