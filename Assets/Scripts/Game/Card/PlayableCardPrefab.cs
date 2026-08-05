using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using Coffee.UIEffects;
using System.Collections;

public class PlayableCardPrefab : CardPrefab, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    public GameObject hideGameObject;

    public GameObject transformationInformation;
    public Transform transformationContainer;
    public GameObject transformationPrefab;
    public UIEffect effectForGui;
    public GameObject innerContainer;
    public Image innerContainerImage;

    void Start()
    {
        GetComponent<UIEffect>()?.LoadPreset("PlayableCardPreset");
        GetComponent<UIEffectTweener>()?.Stop();
    }

    void Update()
    {
        SetCardColor();
    }

    private GameObject contextCardMenuObject;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData == null || eventData.button != PointerEventData.InputButton.Right || card == null)
        {
            return;
        }

            // TODO : Show context menu
            Debug.Log("Right click");
            if(contextCardMenuObject != null){
                var existingRectTransform = contextCardMenuObject.GetComponent<RectTransform>();
                if (existingRectTransform != null)
                {
                    existingRectTransform.position = Input.mousePosition;
                }
                return;
            }

            var prefabDatabase = PrefabDatabase.Instance;
            if (prefabDatabase == null || prefabDatabase.contextCardMenuPrefab == null)
            {
                Debug.LogWarning("Cannot open card context menu: its prefab is missing.");
                return;
            }

            contextCardMenuObject = Instantiate(prefabDatabase.contextCardMenuPrefab, transform);
            contextCardMenuObject.GetComponent<RectTransform>()?.SetPositionAndRotation(Input.mousePosition, Quaternion.identity);
            contextCardMenuObject.GetComponent<PlayableCardContextMenu>()?.SetupMenu(card);
        }

    public override void SetupCard(Card card)
    {
        if (innerContainerImage != null && card != null)
        {
            innerContainerImage.color = card.rarity.GetRarityColor();
        }
        if (transformationInformation != null)
        {
            transformationInformation.SetActive(false);
        }

        base.SetupCard(card);
        if (effectForGui != null)
        {
            if(card != null && card.uiEffectPreset != null && card.uiEffectPreset.Length != 0){
                effectForGui.LoadPreset(card.uiEffectPreset);
            }else{
                effectForGui.LoadPreset("None");
            }
        }
    }

    public void SetCardColor(){
        if(card != null && hideGameObject != null) hideGameObject.SetActive(!card.CanUseCard());
    }

    public void UseCard(){
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (card == null)
        {
            return;
        }

        try {
            card.OnBeginDrag(eventData);
        } catch(Exception error){
            Debug.Log("Error on begin dragging card, e : " + error.ToString());
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (card == null)
        {
            return;
        }

        try {
            card.OnDrag(eventData);
        } catch(Exception error){
            Debug.Log("Error on dragging card, e : " + error.ToString());
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (card == null)
        {
            return;
        }

        try {
            card.OnEndDrag(eventData);
        } catch(Exception error){
            Debug.Log("Error on end dragging card, e : " + error.ToString());
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(card != null && card.CanUseCard() == true){
            GetComponent<UIEffectTweener>()?.PlayForward();
            if (innerContainer != null)
            {
                LeanTween.cancel(innerContainer);
                LeanTween.moveLocalY(innerContainer, 40f, 0.2f).setEaseOutCirc();
            }
        }
        if(card is TransformationCard transfoCard){

            if(card.CanUseCard() == false){
                transformationInformation?.SetActive(false);
                return;
            }
            transformationInformation?.SetActive(true);
            if (transformationContainer == null || transformationPrefab == null)
            {
                return;
            }

            foreach (Transform child in transformationContainer)
            {
                Destroy(child.gameObject);
            }

            foreach(var transfo in transfoCard.transformations ?? Array.Empty<TransformationsPossible>()){
                if (transfo?.character == null || transfo.transformation?.newCharacterData == null)
                {
                    continue;
                }

                var goTransfo = Instantiate(transformationPrefab, transformationContainer);
                var transformationContainerScript = goTransfo.GetComponent<TransformationContainer>();
                if (transformationContainerScript == null)
                {
                    continue;
                }

                transformationContainerScript.characterImage.sprite = transfo.character.characterIcon;
                transformationContainerScript.characterToImage.sprite = transfo.transformation.newCharacterData.characterIcon;
                
                var character = GameManager.Instance.GetCharactersOnBoard()
                .Where(cha => cha?.character != null && cha.character.isPlayerCharacter)
                .ToList()
                .Find(cha => {
                    return transfo.character == cha.character.GetCharacterData();
                });

                if(character == null){
                    transformationContainerScript.characterImageBlack?.gameObject.SetActive(true);
                    transformationContainerScript.characterToImageBlack?.gameObject.SetActive(true);
                    transformationContainerScript.arrowImageBlack?.gameObject.SetActive(true);
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(contextCardMenuObject != null){
            Destroy(contextCardMenuObject);
        }
        GetComponent<UIEffect>()?.LoadPreset("PlayableCardPreset");
        if (innerContainer != null)
        {
            LeanTween.cancel(innerContainer);
            LeanTween.moveLocalY(innerContainer, 0f, 0.2f).setEaseInCirc();
        }
        GetComponent<UIEffectTweener>()?.SetPause(true);
        transformationInformation?.SetActive(false);
    }
}
