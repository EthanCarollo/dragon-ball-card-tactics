using TMPro;
using UnityEngine;

public class HistoryContainer : MonoBehaviour {
    public Transform historyFightContainer;

    public void Start(){
        foreach(Transform child in historyFightContainer){
            Destroy(child.gameObject);
        }

        foreach(var history in HistoryDatabase.Instance.history){
            Instantiate(PrefabDatabase.Instance.historyPrefab, historyFightContainer)
            .GetComponent<HistoryFightGameObject>().Setup(history);
        }
    }
}