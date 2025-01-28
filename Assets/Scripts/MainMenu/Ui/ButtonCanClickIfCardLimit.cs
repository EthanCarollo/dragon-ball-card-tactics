using UnityEngine;
using UnityEngine.UI;

class ButtonCanClickIfCardLimit : MonoBehaviour {
    public void Update(){
        if(CardDatabase.Instance.playerCards.Length == 0){
            GetComponent<Button>().interactable = false;
        }else{
            GetComponent<Button>().interactable = true;
        }
    }
}