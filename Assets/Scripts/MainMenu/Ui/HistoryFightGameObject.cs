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
        if (historyActionGameObject == null)
        {
            return;
        }

        historyActionGameObject.SetActive(!historyActionGameObject.activeSelf);
        var parentRectTransform = transform.parent == null ? null : transform.parent.GetComponent<RectTransform>();
        if(parentRectTransform != null) LayoutRebuilder.ForceRebuildLayoutImmediate(parentRectTransform);
        var historyRectTransform = historyFightContainer == null ? null : historyFightContainer.GetComponent<RectTransform>();
        if(historyRectTransform != null) LayoutRebuilder.ForceRebuildLayoutImmediate(historyRectTransform);
    }

    public void Setup(HistoryFight history, Transform historyFightContainer)
    {
        if (history == null)
        {
            Debug.LogWarning("Cannot setup a history fight UI from a null history entry.");
            return;
        }

        this.historyFightContainer = historyFightContainer;
        int minutes = Mathf.FloorToInt(history.seconds / 60);
        int seconds = Mathf.FloorToInt(history.seconds % 60);

        if (timeText != null)
        {
            timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        if (roundText != null)
        {
            roundText.text = "Round : " + history.round.ToString();
        }

        if (characterContainer != null)
        {
            foreach(Transform child in characterContainer)
            {
                Destroy(child.gameObject);
            }
        }

        var prefabDatabase = PrefabDatabase.Instance;
        if (characterContainer != null && prefabDatabase != null && prefabDatabase.littleCharacterContainer != null)
        {
            foreach(var character in history.characters ?? new CharacterContainer[0])
            {
                if (character == null)
                {
                    continue;
                }

                var characterData = character.GetCharacterData();
                if (characterData == null)
                {
                    continue;
                }

                var go = Instantiate(prefabDatabase.littleCharacterContainer, characterContainer);
                if (go.transform.childCount == 0)
                {
                    continue;
                }

                var image = go.transform.GetChild(0).GetComponent<Image>();
                if (image != null)
                {
                    image.sprite = characterData.characterIcon;
                }
            }
        }

        if (historyActionGameObject == null)
        {
            return;
        }

        foreach(Transform child in historyActionGameObject.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var historyAction in history.historyActions ?? new HistoryAction[0])
        {
            if (historyAction == null)
            {
                continue;
            }

            try {
                historyAction.CreateGameObject(historyActionGameObject.transform);
            } catch(Exception error){
                Debug.LogWarning("Error creating game object: " + error);
            }
        }

        var historyRectTransform = this.historyFightContainer == null
            ? null
            : this.historyFightContainer.GetComponent<RectTransform>();
        if (historyRectTransform != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(historyRectTransform);
        }
    }

    
}
