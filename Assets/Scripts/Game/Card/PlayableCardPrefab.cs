using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using Coffee.UIExtensions;
using System.Collections;

public class PlayableCardPrefab : CardPrefab, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler {
    public GameObject hideGameObject;

    public GameObject transformationInformation;
    public Transform transformationContainer;
    public GameObject transformationPrefab;
    public ShinyEffectForUGUI shinyEffectForUGUI;

    void Update()
    {
        SetCardColor();
    }

    public override void SetupCard(Card card)
    {
        transformationInformation.SetActive(false);
        base.SetupCard(card);
        StartCoroutine(PlayShinyEffect());
    }

    private IEnumerator PlayShinyEffect()
    {
        while (card.highlight)
        {
            shinyEffectForUGUI.Play(0.75f);
            yield return new WaitForSeconds(1.5f); // Wait for 2 seconds
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
        transformationInformation.SetActive(false);
    }
}