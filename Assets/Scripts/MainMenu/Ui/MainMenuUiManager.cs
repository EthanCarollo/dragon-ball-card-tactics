using UnityEngine;

public class MainMenuUiManager : MonoBehaviour
{
        public GameObject MainMenu;
        public GameObject SelectCardMenu;
        public GameObject HistoryMenu;

        public void GoToSelectCardMenu()
        {
                MoveMenu(HistoryMenu, new Vector2(-1920*2, 0));
                MoveMenu(MainMenu, new Vector2(-1920, 0));
                MoveMenu(SelectCardMenu, Vector2.zero);
        }

        public void GoToMainMenu()
        {
                MoveMenu(HistoryMenu, new Vector2(-1920, 0));
                MoveMenu(MainMenu, Vector2.zero);
                MoveMenu(SelectCardMenu, new Vector2(1920, 0));
        }

        public void GoToHistoryMenu()
        {
                MoveMenu(HistoryMenu, Vector2.zero);
                MoveMenu(MainMenu, new Vector2(1920, 0));
                MoveMenu(SelectCardMenu, new Vector2(1920*2, 0));
        }

        private static void MoveMenu(GameObject menu, Vector2 position)
        {
                var rectTransform = menu == null ? null : menu.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                        LeanTween.move(rectTransform, position, 0.4f).setEaseInOutCirc();
                }
        }
}
