using UnityEngine;

public class LoosePanelUiManager : MonoBehaviour {
    public static LoosePanelUiManager Instance;

    public Transform looseTransform;

    public void Start(){
        Instance = this;
        looseTransform.gameObject.SetActive(false);
    }

    public void ShowLoosePanel(){
        looseTransform.gameObject.SetActive(true);
    }
}