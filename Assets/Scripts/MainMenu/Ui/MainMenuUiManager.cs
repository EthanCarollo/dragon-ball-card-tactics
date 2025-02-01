using UnityEngine;

public class MainMenuUiManager : MonoBehaviour
{
        public GameObject MainMenu;
        public GameObject SelectCardMenu;
        public GameObject HistoryMenu;

        public void GoToSelectCardMenu()
        {
                LeanTween.move(HistoryMenu.GetComponent<RectTransform>(), new Vector2(-1920*2, 0), 0.4f)
                        .setEaseInOutCirc();
                LeanTween.move(MainMenu.GetComponent<RectTransform>(), new Vector2(-1920, 0), 0.4f)
                        .setEaseInOutCirc();
                LeanTween.move(SelectCardMenu.GetComponent<RectTransform>(), new Vector2(0, 0), 0.4f)
                        .setEaseInOutCirc();
        }

        public void GoToMainMenu()
        {
                LeanTween.move(HistoryMenu.GetComponent<RectTransform>(), new Vector2(-1920, 0), 0.4f)
                        .setEaseInOutCirc();
                LeanTween.move(MainMenu.GetComponent<RectTransform>(), new Vector2(0, 0), 0.4f)
                        .setEaseInOutCirc();
                LeanTween.move(SelectCardMenu.GetComponent<RectTransform>(), new Vector2(1920, 0), 0.4f)
                        .setEaseInOutCirc();
        }

        public void GoToHistoryMenu()
        {
                LeanTween.move(HistoryMenu.GetComponent<RectTransform>(), new Vector2(0, 0), 0.4f)
                        .setEaseInOutCirc();
                LeanTween.move(MainMenu.GetComponent<RectTransform>(), new Vector2(1920, 0), 0.4f)
                        .setEaseInOutCirc();
                LeanTween.move(SelectCardMenu.GetComponent<RectTransform>(), new Vector2(1920*2, 0), 0.4f)
                        .setEaseInOutCirc();
        }
}