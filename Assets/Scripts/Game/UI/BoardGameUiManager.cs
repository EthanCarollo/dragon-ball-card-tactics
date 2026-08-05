using System;
using System.Linq;
using System.Collections.Generic;
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

    public DropRateBox[] dropRateBoxes;

    public Transform lifeContainer;
    public GameObject lifeGameObject;

    public GameObject launchFightButton;
    public TextMeshProUGUI multiplicatorText;
    public TextMeshProUGUI characterText;
    public TextMeshProUGUI looseManaText;

    public Transform synergyContainer;
    public GameObject synergyPrefab;
    public GameObject synergyPaginationContainer;
    public TextMeshProUGUI currentPageSynergyText;
    private int currentPageSynergy = 0;               
    private const int itemsPerPage = 7; 
    
    public FightNameUi fightNameUi;

    public Transform roundIndicatorContainer;
    public Transform roundIndicatorIcon;
    
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    public void Start()
    {

    }

    public void SetupRoundText(int roundNumber)
    {
        if (roundIndicatorContainer != null)
        {
            foreach(Transform children in roundIndicatorContainer)
            {
                Destroy(children.gameObject);
            }
        }

        var maxRound = 7;
        if (roundIndicatorIcon != null && roundIndicatorContainer != null)
        {
            for (int i = 0; i < maxRound; i++)
            {
                var goRoundIndicator = Instantiate(roundIndicatorIcon, roundIndicatorContainer);
                var image = goRoundIndicator.GetComponent<Image>();
                if (image == null)
                {
                    continue;
                }

                if((roundNumber + i) % 12 == 0){
                    image.color = new Color(1f, 0.2f, 0.2f);
                } else if((roundNumber + i) % 6 == 0){
                    image.color = new Color(1f, 0.7f, 0.7f);
                } else if((roundNumber + i) % 3 == 0){
                    image.color = new Color(0.7f, 1f, 1f);
                } else {
                    image.color = new Color(0.7f, 0.7f, 0.7f);
                }
            }
        }

        if (roundText != null)
        {
            roundText.text = "Round " + roundNumber.ToString();
        }
    }

    public void RefreshUI(){
        if (GameManager.Instance == null)
        {
            return;
        }

        SetupLife();
        SetupSynergy();
        SetupDropRateText();
        SetupMultiplicatorText();
        SetupCharacterText();
        SetupManaSlider(GameManager.Instance.Player.Mana.CurrentMana);
        SetupLevelSlider(GameManager.Instance.Player.Level.CurrentExperience, GameManager.Instance.Player.Level.MaxExperience, GameManager.Instance.Player.Level.CurrentLevel);
    }

    public void SetupCharacterText(){
        if (characterText == null)
        {
            return;
        }

        characterText.text = GameManager.Instance.GetCharactersOnBoard()
            .Count(character => character?.character != null && character.character.isPlayerCharacter) +
            "/" + GameManager.Instance.Player.Level.maxUnit;
    }

    
    public void SetupSynergy()
    {
        try
        {
            currentPageSynergy = 0; // Réinitialiser à la première page
            if (synergyPaginationContainer != null)
            {
                synergyPaginationContainer.SetActive(true);
            }

            // Si le nombre total d'éléments est inférieur ou égal à itemsPerPage, cacher la pagination
            var synergies = GameManager.Instance.GetActiveSynergy();
            if (synergyPaginationContainer != null && synergies.Count <= itemsPerPage)
            {
                synergyPaginationContainer.SetActive(false);
            }

            // Afficher la première page
            DisplayCurrentPageSynergy();
        }
        catch (Exception error)
        {
            Debug.LogError("Error on setup synergies : " + error);
        }
    }

    private void DisplayCurrentPageSynergy()
    {
        if (synergyContainer == null || synergyPrefab == null)
        {
            return;
        }

        // Supprimer les anciens éléments
        foreach (Transform item in synergyContainer)
        {
            Destroy(item.gameObject);
        }

        // Calculer les indices de début et de fin pour la page actuelle
        int startIndex = currentPageSynergy * itemsPerPage;
        var synergies = GameManager.Instance.GetActiveSynergy();
        int endIndex = Mathf.Min(startIndex + itemsPerPage, synergies.Count);

        // Ajouter les synergies de la page actuelle
        for (int i = startIndex; i < endIndex; i++)
        {
            var go = Instantiate(synergyPrefab, synergyContainer);
            go.GetComponent<SynergyPrefabScript>()?.Setup(synergies[i]);
        }
    }

    public void GoNextSynergyPage()
    {
        // Vérifier s'il reste une page suivante
        if ((currentPageSynergy + 1) * itemsPerPage < GameManager.Instance.GetActiveSynergy().Count)
        {
            currentPageSynergy++;
            DisplayCurrentPageSynergy();
            if (currentPageSynergyText != null)
            {
                currentPageSynergyText.text = currentPageSynergy.ToString();
            }
        }
    }

    public void GoBackSynergyPage()
    {
        // Vérifier s'il existe une page précédente
        if (currentPageSynergy > 0)
        {
            currentPageSynergy--;
            DisplayCurrentPageSynergy();
            if (currentPageSynergyText != null)
            {
                currentPageSynergyText.text = currentPageSynergy.ToString();
            }
        }
    }

    public void SetupMultiplicatorText(){
        if (multiplicatorText != null)
        {
            multiplicatorText.text = "actual difficulty multiplicator : " + string.Format("{0:F2}", GameManager.Instance.difficultyMutliplicator) + "x";
        }
    }

    public void SetupLife(){
        if (lifeContainer == null || lifeGameObject == null)
        {
            return;
        }

        foreach (Transform item in lifeContainer)
        {  
            Destroy(item.gameObject); 
        }
        var spriteDatabase = SpriteDatabase.Instance;
        for (int i = 1; i <= Mathf.Max(0, GameManager.Instance.Player.Life.MaxLife); i++)
        {
            var go = Instantiate(lifeGameObject, lifeContainer);
            var image = go.GetComponent<Image>();
            if (image == null || spriteDatabase == null)
            {
                continue;
            }

            if(GameManager.Instance.Player.Life.CurrentLife >= i){
                image.sprite = spriteDatabase.fullfillHeart;
            } else {
                image.sprite = spriteDatabase.emptyHeart;
            }
        }
    }

    private void SetupDropRateText(){
        foreach (var item in dropRateBoxes ?? Array.Empty<DropRateBox>())
        {
            item?.SetupBox();
        }
    }

    private void SetupManaSlider(int manaValue)
    {
        if (manaSlider != null)
        {
            manaSlider.value = manaValue;
        }
        if (manaText != null)
        {
            manaText.text = "Mana : " + manaValue.ToString() + "/" +
                (manaSlider == null ? 0 : manaSlider.maxValue).ToString();
        }
    }

    private void SetupLevelSlider(int expValue, int maxLevelValue, int levelValue)
    {
        if (levelSlider != null)
        {
            levelSlider.value = expValue;
            levelSlider.maxValue = maxLevelValue;
        }
        if (levelText != null)
        {
            levelText.text = "Level " + levelValue.ToString();
        }
    }

    public void ShowLooseMana(int amount){
        if (looseManaText == null)
        {
            return;
        }

        looseManaText.gameObject.SetActive(true);
        looseManaText.alpha = 1f;
        var looseManaRectTransform = looseManaText.GetComponent<RectTransform>();
        if (looseManaRectTransform == null) return;
        looseManaRectTransform.anchoredPosition = new Vector2(0f, 0f);

        looseManaText.text = "-" + amount.ToString();

        LeanTween.cancel(looseManaText.gameObject);
        LeanTween.value(looseManaText.gameObject, f => looseManaText.alpha=f,1f,0f, 0.75f).setDelay(0.5f);
        LeanTween.moveX(looseManaRectTransform, 15f, 1.25f).setEaseOutCirc();
    }

    public void ShowPlayCardPanel(string useCardText = "Use Card")
    {
        if (playCardScreen == null)
        {
            return;
        }

        var playCardRectTransform = playCardScreen.GetComponent<RectTransform>();
        if (playCardRectTransform == null)
        {
            return;
        }

        if (playCardScreen.activeInHierarchy == false)
        {
            isTweeningEnd = false;
            LeanTween.cancel(playCardScreen);
            var newPosition = playCardRectTransform.sizeDelta.y;
            playCardRectTransform.localPosition =
                new Vector2(playCardRectTransform.localPosition.x, newPosition);
            playCardScreen.SetActive(true);
            LeanTween.value(playCardScreen, (f) =>
            {
                playCardRectTransform.localPosition =
                    new Vector2(playCardRectTransform.localPosition.x,
                        f);
            }, newPosition, 0f, 0.2f).setEaseInOutCirc();
        }
    }

    private bool isTweeningEnd = false;
    public void HidePlayCardPanel()
    {
        if (playCardScreen == null)
        {
            return;
        }

        var playCardRectTransform = playCardScreen.GetComponent<RectTransform>();
        if (playCardRectTransform == null)
        {
            return;
        }

        if (playCardScreen.activeInHierarchy && isTweeningEnd == false)
        {
            isTweeningEnd = true;
            var newPosition = playCardRectTransform.sizeDelta.y;
            LeanTween.value(playCardScreen, (f) =>
            {
                playCardRectTransform.localPosition =
                    new Vector2(playCardRectTransform.localPosition.x,
                        f);
            }, 0f, newPosition, 0.2f).setEaseInOutCirc().setOnComplete(() =>
            {
                playCardScreen.SetActive(false);
                isTweeningEnd = false;
            });
        }

    }
}
