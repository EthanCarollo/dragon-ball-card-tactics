using UnityEngine;

public class MainMenuUiManager : MonoBehaviour
{
        public GameObject MainMenu;
        public GameObject SelectCardMenu;

        public void GoToSelectCardMenu()
        {
                LeanTween.move(MainMenu.GetComponent<RectTransform>(), new Vector2(-1920, 0), 0.4f)
                        .setEaseInOutCirc();
                LeanTween.move(SelectCardMenu.GetComponent<RectTransform>(), new Vector2(0, 0), 0.4f)
                        .setEaseInOutCirc();
        }

        public void GoToMainMenu()
        {
                LeanTween.move(MainMenu.GetComponent<RectTransform>(), new Vector2(0, 0), 0.4f)
                        .setEaseInOutCirc();
                LeanTween.move(SelectCardMenu.GetComponent<RectTransform>(), new Vector2(1920, 0), 0.4f)
                        .setEaseInOutCirc();
        }
}