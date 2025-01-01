using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class CardPrefab : MonoBehaviour
{
    public Image cardImage;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardMana;
    public TextMeshProUGUI cardDescription;
    protected Card card;

    public virtual void SetupCard(Card card){
        try {
            this.GetComponent<Image>().color = card.rarity.GetRarityColor();
            cardImage.sprite = card.image;
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