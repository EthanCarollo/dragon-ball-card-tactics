using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using Coffee.UIEffects;
using System.Collections;

public class PlayableCardPrefab : CardPrefab, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler {
    public GameObject hideGameObject;

    public GameObject transformationInformation;
    public Transform transformationContainer;
    public GameObject transformationPrefab;
    public UIEffect effectForGui;

    void Start()
    {
        this.GetComponent<UIEffect>().LoadPreset("PlayableCardPreset");
        this.GetComponent<UIEffectTweener>().Stop();
    }

    void Update()
    {
        SetCardColor();
    }

    public override void SetupCard(Card card)
    {
        transformationInformation.SetActive(false);
        base.SetupCard(card);
        if(card.highlight == true){
            effectForGui.LoadPreset("Shiny");
        }
    }

    public void SetCardColor(){
        hideGameObject.SetActive(!card.CanUseCard());
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
        if(card.CanUseCard() == true){
            this.GetComponent<UIEffectTweener>().PlayForward();
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
        this.GetComponent<UIEffect>().LoadPreset("PlayableCardPreset");
        this.GetComponent<UIEffectTweener>().SetPause(true);
        transformationInformation.SetActive(false);
    }
}