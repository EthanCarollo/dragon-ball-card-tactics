using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HistoryFightGameObject : MonoBehaviour {
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI timeText;
    public Transform characterContainer;

    public void Setup(HistoryFight history){
        int minutes = history.seconds / 60;
        int seconds = history.seconds % 60;
        
        timeText.text = minutes.ToString() + ":" + seconds.ToString();
        roundText.text = "Round : " + history.round.ToString();
        foreach(Transform child in characterContainer){
            Destroy(child.gameObject);
        }

        foreach(var character in history.characters){
            var go = Instantiate(PrefabDatabase.Instance.littleCharacterContainer, characterContainer);
            go.transform.GetChild(0).GetComponent<Image>().sprite = character.GetCharacterData().characterIcon;
        }
    }
}