using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SaveButton : MonoBehaviour, IPointerClickHandler
{
        public TextMeshProUGUI text;

        public void Setup()
        {
                text.text = "New Game";
        }

        public void OnPointerClick(PointerEventData eventData)
        {
                Debug.Log("Launched a new save");
        }
}