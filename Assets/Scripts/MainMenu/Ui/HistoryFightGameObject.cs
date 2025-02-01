using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HistoryFightGameObject : MonoBehaviour {
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI timeText;
    public Transform characterContainer;

    public void Setup(HistoryFight history){
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
    }
}