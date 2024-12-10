using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoardGameUiManager : MonoBehaviour
{
    public static BoardGameUiManager Instance;
    public CharacterBoardUi characterBoardUi;
    public GameObject playCardScreen;
    public GameObject draggedCardPrefab;
    public TextMeshProUGUI roundText;
    public Slider manaSlider;
    public TextMeshProUGUI manaText;
    public Slider levelSlider;
    public TextMeshProUGUI levelText;
    
    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {

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

    public void SetupRoundText(string roundNumber)
    {
        try {
            roundText.text = "Round " + roundNumber;
        } catch {
            Debug.Log("Got an error on setup round text");
        }
    }

    public void RefreshSlider(){
        SetupManaSlider(GameManager.Instance.Player.Mana.CurrentMana);
        SetupLevelSlider(GameManager.Instance.Player.Level.CurrentExperience, GameManager.Instance.Player.Level.MaxExperience, GameManager.Instance.Player.Level.CurrentLevel);
    }

    private void SetupManaSlider(int manaValue)
    {
        manaSlider.value = manaValue;
        manaText.text = "Mana : " + manaValue.ToString() + "/" + manaSlider.maxValue.ToString();
    }

    private void SetupLevelSlider(int expValue, int maxLevelValue, int levelValue)
    {
        levelSlider.value = expValue;
        levelSlider.maxValue = maxLevelValue;
        levelText.text = "Level " + levelValue.ToString();
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
