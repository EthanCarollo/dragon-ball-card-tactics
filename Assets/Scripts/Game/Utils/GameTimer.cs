using TMPro;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    void Update()
    {
        GameManager.Instance.elapsedTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(GameManager.Instance.elapsedTime / 60);
        int seconds = Mathf.FloorToInt(GameManager.Instance.elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
