using TMPro;
using UnityEngine;

public class HistoryContainer : MonoBehaviour {
    public Transform historyFightContainer;
    public TextMeshProUGUI nothingText;

    public void Start(){
        if (historyFightContainer == null)
        {
            Debug.LogError("Cannot display history: the history fight container is missing.");
            return;
        }

        var historyDatabase = HistoryDatabase.Instance;
        var prefabDatabase = PrefabDatabase.Instance;
        var history = historyDatabase?.history ?? new HistoryFight[0];
        if (nothingText != null)
        {
            nothingText.gameObject.SetActive(history.Length == 0);
        }

        foreach(Transform child in historyFightContainer)
        {
            Destroy(child.gameObject);
        }

        if (prefabDatabase == null || prefabDatabase.historyPrefab == null)
        {
            Debug.LogError("Cannot display history: history prefab is missing.");
            return;
        }

        foreach(var historyFight in history)
        {
            if (historyFight == null)
            {
                continue;
            }

            var historyObject = Instantiate(prefabDatabase.historyPrefab, historyFightContainer);
            var historyFightObject = historyObject.GetComponent<HistoryFightGameObject>();
            if (historyFightObject == null)
            {
                Debug.LogError("History prefab has no HistoryFightGameObject component.");
                Destroy(historyObject);
                continue;
            }

            historyFightObject.Setup(historyFight, historyFightContainer);
        }
    }
}
