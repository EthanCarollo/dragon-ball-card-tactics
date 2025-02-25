using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class HistoryAction {
    public int time;

    public virtual void CreateGameObject(Transform parent){
        MonoBehaviour.Instantiate(PrefabDatabase.Instance.defaultHistoryActionPrefab, parent);
    }
}

[Serializable]
public class PlayCardHistoryAction : HistoryAction {
    public Card cardPlayed;

    public override void CreateGameObject(Transform parent){
        var go = MonoBehaviour.Instantiate(PrefabDatabase.Instance.playCardHistoryActionPrefab, parent);
        go.GetComponentInChildren<CardPreviewPrefab>().SetupCard(cardPlayed);
        go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Played " + cardPlayed.name + " card";
        go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = string.Format("{0:D2}:{1:D2}", time / 60, time % 60);
        
    }
}

[Serializable]
public class EndFightHistoryAction : HistoryAction {
    public Fight fightEnded;
    public bool winFight;

    public override void CreateGameObject(Transform parent){
        var go = MonoBehaviour.Instantiate(PrefabDatabase.Instance.endFightHistoryActionPrefab, parent);
        go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Ended " + fightEnded.name + " fight";
        go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = string.Format("{0:D2}:{1:D2}", time / 60, time % 60);
        Transform characterContainer = go.transform.GetChild(2);
        foreach(var character in fightEnded.opponents){
            var littleCharGo = MonoBehaviour.Instantiate(PrefabDatabase.Instance.littleCharacterContainer, characterContainer);
            littleCharGo.transform.GetChild(0).GetComponent<Image>().sprite = character.characterData.characterIcon;
        }
    }
}


