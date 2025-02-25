using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HistoryFightGameObject : MonoBehaviour, IPointerClickHandler {
    public Transform historyFightContainer;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI timeText;
    public Transform characterContainer;
    public GameObject historyActionGameObject;

    public void OnPointerClick(PointerEventData eventData)
    {
        historyActionGameObject.SetActive(!historyActionGameObject.activeSelf);
        LayoutRebuilder.ForceRebuildLayoutImmediate(historyFightContainer.GetComponent<RectTransform>());
    }

    public void Setup(HistoryFight history, Transform historyFightContainer){
        this.historyFightContainer = historyFightContainer;
        int minutes = Mathf.FloorToInt(history.seconds / 60);
        int seconds = Mathf.FloorToInt(history.seconds % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        roundText.text = "Round : " + history.round.ToString();
        foreach(Transform child in characterContainer){
            Destroy(child.gameObject);
        }

        foreach(var character in history.characters){
            var go = Instantiate(PrefabDatabase.Instance.littleCharacterContainer, characterContainer);
            go.transform.GetChild(0).GetComponent<Image>().sprite = character.GetCharacterData().characterIcon;
        }

        foreach(Transform child in historyActionGameObject.transform){
            Destroy(child.gameObject);
        }

        foreach (var historyAction in history.historyActions)
        {
            try {
                historyAction.CreateGameObject(historyActionGameObject.transform);
            } catch(Exception error){
                Debug.LogWarning("Error creating game object: " + error);
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(this.historyFightContainer.GetComponent<RectTransform>());
    }

    
}