using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CardPrefab : MonoBehaviour
{
    public Image cardImage;
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI cardMana;
    public TextMeshProUGUI cardDescription;
    protected Card card;

    public void SetupCard(Card card){
        this.GetComponent<Image>().color = card.GetRarityColor();
        cardImage.sprite = card.image;
        cardName.text = card.name;
        cardMana.text = card.manaCost.ToString();
        this.card = card;
        if (this.cardDescription != null)
        {
            this.cardDescription.text = card.GetDescription();
        }
    }

}