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
        this.GetComponent<UIEffect>().LoadPreset("PlayableCardPreset");
        this.GetComponent<UIEffectTweener>().Stop();
    }

    void Update()
    {
        SetCardColor();
    }

    private GameObject contextCardMenuObject;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right) {
            // TODO : Show context menu
            Debug.Log("Right click");
            if(contextCardMenuObject != null){
                contextCardMenuObject.GetComponent<RectTransform>().position = Input.mousePosition;
                return;
            } 
            contextCardMenuObject = Instantiate(PrefabDatabase.Instance.contextCardMenuPrefab, this.transform);
            contextCardMenuObject.GetComponent<RectTransform>().position = Input.mousePosition;
            contextCardMenuObject.GetComponent<PlayableCardContextMenu>().SetupMenu(card);
        }
    }

    public override void SetupCard(Card card)
    {
        innerContainerImage.color = card.rarity.GetRarityColor();
        transformationInformation.SetActive(false);
        base.SetupCard(card);
        if(card.uiEffectPreset != null && card.uiEffectPreset.Length != 0){
            effectForGui.LoadPreset(card.uiEffectPreset);
        }else{
            effectForGui.LoadPreset("None");
        }
    }

    public void SetCardColor(){
        if(card != null && hideGameObject != null) hideGameObject.SetActive(!card.CanUseCard());
    }

    public void UseCard(){
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        try {
            card.OnBeginDrag(eventData);
        } catch(Exception error){
            Debug.Log("Error on begin dragging card, e : " + error.ToString());
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        try {
            card.OnDrag(eventData);
        } catch(Exception error){
            Debug.Log("Error on dragging card, e : " + error.ToString());
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        try {
            card.OnEndDrag(eventData);
        } catch(Exception error){
            Debug.Log("Error on end dragging card, e : " + error.ToString());
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(card != null && card.CanUseCard() == true){
            this.GetComponent<UIEffectTweener>().PlayForward();
            LeanTween.cancel(this.innerContainer);
            LeanTween.moveLocalY(this.innerContainer, 40f, 0.2f).setEaseOutCirc();
        }
        if(card is TransformationCard transfoCard){

            if(card.CanUseCard() == false){
                transformationInformation.SetActive(false);
                return;
            }
            transformationInformation.SetActive(true);
            foreach (Transform child in transformationContainer)
            {
                Destroy(child.gameObject);
            }
            
            foreach(var transfo in transfoCard.transformations){
                var goTransfo = Instantiate(transformationPrefab, transformationContainer);
                goTransfo.GetComponent<TransformationContainer>().characterImage.sprite = transfo.character.characterIcon;
                goTransfo.GetComponent<TransformationContainer>().characterToImage.sprite = transfo.transformation.newCharacterData.characterIcon;
                
                var character = GameManager.Instance.GetCharactersOnBoard()
                .Where(cha => cha.character.isPlayerCharacter)
                .ToList()
                .Find(cha => {
                    return transfo.character == cha.character.GetCharacterData();
                });

                if(character == null){
                    goTransfo.GetComponent<TransformationContainer>().characterImageBlack.gameObject.SetActive(true);
                    goTransfo.GetComponent<TransformationContainer>().characterToImageBlack.gameObject.SetActive(true);
                    goTransfo.GetComponent<TransformationContainer>().arrowImageBlack.gameObject.SetActive(true);
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(contextCardMenuObject != null){
            Destroy(contextCardMenuObject);
        }
        this.GetComponent<UIEffect>().LoadPreset("PlayableCardPreset");
        LeanTween.cancel(this.innerContainer);
        LeanTween.moveLocalY(this.innerContainer, 0f, 0.2f).setEaseInCirc();
        this.GetComponent<UIEffectTweener>().SetPause(true);
        transformationInformation.SetActive(false);
    }
}