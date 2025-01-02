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

    protected Card card;

    public virtual void SetupCard(Card card){
        try {
            this.GetComponent<Image>().color = card.rarity.GetRarityColor();
            cardImage.sprite = card.image;
            if(fullArtImage != null && card.fullartImage != null){
                cardImage.gameObject.SetActive(false);
                fullArtImage.gameObject.SetActive(true);
                fullArtImage.sprite = card.fullartImage;
                fullArtCardName.text = card.name;
                cardTextContainer.SetActive(false);
            }else{
                if(fullArtImage != null){
                    fullArtImage.gameObject.SetActive(false);
                    cardTextContainer.SetActive(true);
                }
                cardImage.gameObject.SetActive(true);
            }
            cardName.text = card.name;
            cardMana.text = card.manaCost.ToString();
            this.card = card;
            if (this.cardDescription != null)
            {
                this.cardDescription.text = card.GetDescription();
            }
        } catch(Exception error){
            Debug.LogError("Error while setuping card : " + error);
        }
    }

}