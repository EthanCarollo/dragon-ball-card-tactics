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
    public int cardPlayedId;

    public Card GetCard(){
        return CardDatabase.Instance.cards[cardPlayedId];
    }

    public override void CreateGameObject(Transform parent){
        var go = MonoBehaviour.Instantiate(PrefabDatabase.Instance.playCardHistoryActionPrefab, parent);
        go.GetComponentInChildren<CardPreviewPrefab>().SetupCard(GetCard());
        go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Played " + GetCard().name + " card";
        go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = string.Format("{0:D2}:{1:D2}", time / 60, time % 60);
        
    }
}

[Serializable]
public class EndFightHistoryAction : HistoryAction {
    public int fightEndedId;
    public bool winFight;

    public Fight GetFight(){
        return FightDatabase.Instance.fights[fightEndedId];
    }

    public override void CreateGameObject(Transform parent){
        var go = MonoBehaviour.Instantiate(PrefabDatabase.Instance.endFightHistoryActionPrefab, parent);
        go.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Ended " + GetFight().name + " fight";
        go.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = string.Format("{0:D2}:{1:D2}", time / 60, time % 60);
        Transform characterContainer = go.transform.GetChild(2);
        foreach(var character in GetFight().opponents){
            var littleCharGo = MonoBehaviour.Instantiate(PrefabDatabase.Instance.littleCharacterContainer, characterContainer);
            littleCharGo.transform.GetChild(0).GetComponent<Image>().sprite = character.characterData.characterIcon;
        }
    }
}


