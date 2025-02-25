using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class CardPrefab : MonoBehaviour
{
    public Image cardImage;
    public GameObject cardTextContainer;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardMana;
    public TextMeshProUGUI cardDescription;

    public Image fullArtImage;
    public TextMeshProUGUI fullArtCardName;
    public TextMeshProUGUI fullArtCardDescription;

    protected Card card;

    public virtual void SetupCard(Card card){
        try
        {
            this.GetComponent<Image>().color = card.rarity.GetRarityColor();
        }
        catch (Exception error)
        {
            Debug.Log("Image isn't set on the main card prefab" + "  " + error);   
        }
        try {
            cardImage.sprite = card.image;
            if(fullArtImage != null && card.fullartImage != null){
                if(cardImage != null) cardImage.gameObject.SetActive(false);
                if(fullArtImage != null) {
                    fullArtImage.gameObject.SetActive(true);
                    fullArtImage.sprite = card.fullartImage;
                }
                if(fullArtCardName != null) fullArtCardName.text = card.name;
                if(fullArtCardDescription != null) fullArtCardDescription.text = card.GetDescription();
                cardTextContainer.SetActive(false);
            }else{
                if(fullArtImage != null){
                    fullArtImage.gameObject.SetActive(false);
                    if(cardTextContainer != null) cardTextContainer.SetActive(true);
                }
                if(cardImage != null) cardImage.gameObject.SetActive(true);
            }
            if(cardName != null) cardName.text = card.name;
            if(cardMana != null) cardMana.text = card.manaCost.ToString();
            this.card = card;
            if(this.cardDescription != null) this.cardDescription.text = card.GetDescription();
            
        } catch(Exception error){
            Debug.LogError("Error while setuping card : " + error);
        }
    }

}