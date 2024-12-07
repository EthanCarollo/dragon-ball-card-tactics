using UnityEngine;

public class BoardGameUiManager : MonoBehaviour
{
    public static BoardGameUiManager Instance;
    public CharacterBoardUi characterBoardUi;
    public GameObject playCardScreen;
    public GameObject draggedCardPrefab;
    
    public void Awake()
    {
        Instance = this;
    }

    public void ShowPlayCardPanel()
    {
        if (playCardScreen.activeInHierarchy == false)
        {
            isTweeningEnd = false;
            LeanTween.cancel(playCardScreen);
            var newPosition = playCardScreen.GetComponent<RectTransform>().sizeDelta.y;
            playCardScreen.GetComponent<RectTransform>().localPosition = 
                new Vector2(playCardScreen.GetComponent<RectTransform>().localPosition.x, newPosition);
            playCardScreen.SetActive(true);
            LeanTween.value(playCardScreen, (f) =>
            {
                playCardScreen.GetComponent<RectTransform>().localPosition =
                    new Vector2(playCardScreen.GetComponent<RectTransform>().localPosition.x,
                        f);
            }, newPosition, 0f, 0.2f).setEaseInOutCirc();
        }
    }

    private bool isTweeningEnd = false;
    public void HidePlayCardPanel()
    {
        if (playCardScreen.activeInHierarchy && isTweeningEnd == false)
        {
            isTweeningEnd = true;
            var newPosition = playCardScreen.GetComponent<RectTransform>().sizeDelta.y;
            LeanTween.value(playCardScreen, (f) =>
            {
                playCardScreen.GetComponent<RectTransform>().localPosition =
                    new Vector2(playCardScreen.GetComponent<RectTransform>().localPosition.x,
                        f);
            }, 0f, newPosition, 0.2f).setEaseInOutCirc().setOnComplete(() =>
            {
                playCardScreen.SetActive(false);
                isTweeningEnd = false;
            });
        }

    }
}
