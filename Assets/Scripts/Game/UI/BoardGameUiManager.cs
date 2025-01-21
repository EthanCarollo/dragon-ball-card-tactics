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
        Instance = this;
    }

    public void Start()
    {

    }

    public void SetupRoundText(int roundNumber)
    {
        foreach(Transform children in roundIndicatorContainer)
        {
            Destroy(children.gameObject);
        }

        var maxRound = 7;
        for (int i = 0; i < maxRound; i++)
        {
            var goRoundIndicator = Instantiate(roundIndicatorIcon, roundIndicatorContainer);
            if((roundNumber + i) % 12 == 0){
                goRoundIndicator.GetComponent<Image>().color = new Color(1f, 0.2f, 0.2f);
            } else if((roundNumber + i) % 6 == 0){
                goRoundIndicator.GetComponent<Image>().color = new Color(1f, 0.7f, 0.7f);
            } else if((roundNumber + i) % 3 == 0){
                goRoundIndicator.GetComponent<Image>().color = new Color(0.7f, 1f, 1f);
            } else {
                goRoundIndicator.GetComponent<Image>().color = new Color(0.7f, 0.7f, 0.7f);
            }
        }

        try {
            roundText.text = "Round " + roundNumber.ToString();
        } catch {
            Debug.Log("Got an error on setup round text");
        }
    }

    public void RefreshUI(){
        SetupLife();
        SetupSynergy();
        SetupDropRateText();
        SetupMultiplicatorText();
        SetupCharacterText();
        SetupManaSlider(GameManager.Instance.Player.Mana.CurrentMana);
        SetupLevelSlider(GameManager.Instance.Player.Level.CurrentExperience, GameManager.Instance.Player.Level.MaxExperience, GameManager.Instance.Player.Level.CurrentLevel);
    }

    public void SetupCharacterText(){
        characterText.text = GameManager.Instance.GetCharactersOnBoard().Where(character => character.isPlayerCharacter).Count() + "/" + GameManager.Instance.Player.Level.maxUnit;
    }

    
    public void SetupSynergy()
    {
        try
        {
            currentPageSynergy = 0; // Réinitialiser à la première page
            synergyPaginationContainer.SetActive(true);

            // Si le nombre total d'éléments est inférieur ou égal à itemsPerPage, cacher la pagination
            if (GameManager.Instance.GetActiveSynergy().Count <= itemsPerPage)
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
        // Supprimer les anciens éléments
        foreach (Transform item in synergyContainer)
        {
            Destroy(item.gameObject);
        }

        // Calculer les indices de début et de fin pour la page actuelle
        int startIndex = currentPageSynergy * itemsPerPage;
        int endIndex = Mathf.Min(startIndex + itemsPerPage, GameManager.Instance.GetActiveSynergy().Count);

        // Ajouter les synergies de la page actuelle
        for (int i = startIndex; i < endIndex; i++)
        {
            var go = Instantiate(synergyPrefab, synergyContainer);
            go.GetComponent<SynergyPrefabScript>().Setup(GameManager.Instance.GetActiveSynergy()[i]);
        }
    }

    public void GoNextSynergyPage()
    {
        // Vérifier s'il reste une page suivante
        if ((currentPageSynergy + 1) * itemsPerPage < GameManager.Instance.GetActiveSynergy().Count)
        {
            currentPageSynergy++;
            DisplayCurrentPageSynergy();
            currentPageSynergyText.text = currentPageSynergy.ToString();
        }
    }

    public void GoBackSynergyPage()
    {
        // Vérifier s'il existe une page précédente
        if (currentPageSynergy > 0)
        {
            currentPageSynergy--;
            DisplayCurrentPageSynergy();
            currentPageSynergyText.text = currentPageSynergy.ToString();
        }
    }

    public void SetupMultiplicatorText(){
        multiplicatorText.text = "actual difficulty multiplicator : " + string.Format("{0:F2}", GameManager.Instance.difficultyMutliplicator) + "x";
    }

    public void SetupLife(){
        foreach (Transform item in lifeContainer)
        {  
            Destroy(item.gameObject); 
        }
        for (int i = 1; i <= GameManager.Instance.Player.Life.MaxLife; i++)
        {
            var go = Instantiate(lifeGameObject, lifeContainer);
            if(GameManager.Instance.Player.Life.CurrentLife >= i){
                go.GetComponent<Image>().sprite = SpriteDatabase.Instance.fullfillHeart;
            } else {
                go.GetComponent<Image>().sprite = SpriteDatabase.Instance.emptyHeart;
            }
        }
    }

    private void SetupDropRateText(){
        foreach (var item in dropRateBoxes)
        {
            item.SetupBox();
        }
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

    public void ShowLooseMana(int amount){
        looseManaText.gameObject.SetActive(true);
        looseManaText.alpha = 1f;
        looseManaText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);

        looseManaText.text = "-" + amount.ToString();

        LeanTween.cancel(looseManaText.gameObject);
        LeanTween.value(looseManaText.gameObject, f => looseManaText.alpha=f,1f,0f, 0.75f).setDelay(0.5f);
        LeanTween.moveX(looseManaText.gameObject.GetComponent<RectTransform>(), 15f, 1.25f).setEaseOutCirc();
    }

    public void ShowPlayCardPanel(string useCardText = "Use Card")
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
