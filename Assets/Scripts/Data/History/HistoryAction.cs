using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class HistoryAction {
    public int time;

    public virtual void CreateGameObject(Transform parent)
    {
        if (parent == null)
        {
            Debug.LogWarning("Cannot display a history action without a parent transform.");
            return;
        }

        var prefabDatabase = PrefabDatabase.Instance;
        if (prefabDatabase == null || prefabDatabase.defaultHistoryActionPrefab == null)
        {
            Debug.LogWarning("Cannot display the history action: default history action prefab is missing.");
            return;
        }

        MonoBehaviour.Instantiate(prefabDatabase.defaultHistoryActionPrefab, parent);
    }
}

[Serializable]
public class PlayCardHistoryAction : HistoryAction {
    public int cardPlayedId;

    public Card GetCard(){
        var database = CardDatabase.Instance;
        if (database == null || database.cards == null || cardPlayedId < 0 || cardPlayedId >= database.cards.Length)
        {
            return null;
        }

        return database.cards[cardPlayedId];
    }

    public override void CreateGameObject(Transform parent)
    {
        var card = GetCard();
        if (card == null)
        {
            Debug.LogWarning($"Cannot display card history action: card ID {cardPlayedId} is invalid.");
            base.CreateGameObject(parent);
            return;
        }

        var prefabDatabase = PrefabDatabase.Instance;
        if (parent == null || prefabDatabase == null || prefabDatabase.playCardHistoryActionPrefab == null)
        {
            Debug.LogWarning("Cannot display card history action: required prefab or parent is missing.");
            return;
        }

        var go = MonoBehaviour.Instantiate(prefabDatabase.playCardHistoryActionPrefab, parent);
        var cardPreview = go.GetComponentInChildren<CardPreviewPrefab>();
        if (cardPreview != null)
        {
            cardPreview.SetupCard(card);
        }

        SetChildText(go.transform, 0, "Played " + card.name + " card");
        SetChildText(go.transform, 1, FormatTime());
    }

    private string FormatTime()
    {
        return string.Format("{0:D2}:{1:D2}", Mathf.Max(0, time) / 60, Mathf.Max(0, time) % 60);
    }

    private static void SetChildText(Transform parent, int childIndex, string value)
    {
        if (parent == null || childIndex < 0 || childIndex >= parent.childCount)
        {
            return;
        }

        var text = parent.GetChild(childIndex).GetComponent<TextMeshProUGUI>();
        if (text != null)
        {
            text.text = value;
        }
    }
}

[Serializable]
public class EndFightHistoryAction : HistoryAction {
    public int fightEndedId;
    public bool winFight;

    public Fight GetFight(){
        var database = FightDatabase.Instance;
        if (database == null || database.fights == null || fightEndedId < 0 || fightEndedId >= database.fights.Length)
        {
            return null;
        }

        return database.fights[fightEndedId];
    }

    public override void CreateGameObject(Transform parent)
    {
        var fight = GetFight();
        if (fight == null)
        {
            Debug.LogWarning($"Cannot display fight history action: fight ID {fightEndedId} is invalid.");
            base.CreateGameObject(parent);
            return;
        }

        var prefabDatabase = PrefabDatabase.Instance;
        if (parent == null || prefabDatabase == null || prefabDatabase.endFightHistoryActionPrefab == null)
        {
            Debug.LogWarning("Cannot display fight history action: required prefab or parent is missing.");
            return;
        }

        var go = MonoBehaviour.Instantiate(prefabDatabase.endFightHistoryActionPrefab, parent);
        SetChildText(go.transform, 0, "Ended " + fight.name + " fight");
        SetChildText(go.transform, 1, FormatTime());

        if (fight.opponents == null || prefabDatabase.littleCharacterContainer == null || go.transform.childCount <= 2)
        {
            return;
        }

        Transform characterContainer = go.transform.GetChild(2);
        foreach (var opponent in fight.opponents)
        {
            if (opponent == null || opponent.characterData == null)
            {
                continue;
            }

            var littleCharGo = MonoBehaviour.Instantiate(prefabDatabase.littleCharacterContainer, characterContainer);
            if (littleCharGo.transform.childCount == 0)
            {
                continue;
            }

            var image = littleCharGo.transform.GetChild(0).GetComponent<Image>();
            if (image != null)
            {
                image.sprite = opponent.characterData.characterIcon;
            }
        }
    }

    private string FormatTime()
    {
        return string.Format("{0:D2}:{1:D2}", Mathf.Max(0, time) / 60, Mathf.Max(0, time) % 60);
    }

    private static void SetChildText(Transform parent, int childIndex, string value)
    {
        if (parent == null || childIndex < 0 || childIndex >= parent.childCount)
        {
            return;
        }

        var text = parent.GetChild(childIndex).GetComponent<TextMeshProUGUI>();
        if (text != null)
        {
            text.text = value;
        }
    }
}


