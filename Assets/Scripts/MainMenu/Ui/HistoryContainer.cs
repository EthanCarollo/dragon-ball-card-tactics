using TMPro;
using UnityEngine;

public class HistoryContainer : MonoBehaviour {
    public Transform historyFightContainer;
    public TextMeshProUGUI nothingText;

    public void Start(){
        if(HistoryDatabase.Instance.history.Length == 0){
            nothingText.gameObject.SetActive(true);
        }else{
            nothingText.gameObject.SetActive(false);
        }
        foreach(Transform child in historyFightContainer){
            Destroy(child.gameObject);
        }

        foreach(var history in HistoryDatabase.Instance.history){
            Instantiate(PrefabDatabase.Instance.historyPrefab, historyFightContainer)
            .GetComponent<HistoryFightGameObject>().Setup(history, historyFightContainer);
        }
    }
}