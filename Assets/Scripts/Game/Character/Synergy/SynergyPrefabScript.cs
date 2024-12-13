using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SynergyPrefabScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public Synergy synergy;
    public TextMeshProUGUI synergyNumber;
    public GameObject tierDescription;
    public TextMeshProUGUI descriptionText;
    public Image synergyImage;

    public void Setup(Synergy synergy) {
        this.synergy = synergy;
        synergyNumber.text = synergy.GetActiveUnit().ToString();
        descriptionText.text = synergy.GetDescription();
        synergyImage.sprite = synergy.synergyImage;
        var tierBonuses = synergy.GetActiveTierBonuses();
        if(tierBonuses.ToArray().Length == 0){
            GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f);
            transform.GetChild(0).GetComponent<Image>().color = new Color(0.4f, 0.4f, 0.4f);
        } else {
            switch(tierBonuses.ToArray().Length){
                case 1: 
                    GetComponent<Image>().color = new Color(0.5f, 0.4f, 0.3f);
                    break;
                case 2: 
                    GetComponent<Image>().color = new Color(1f, 0.9f, 0.95f);
                    break;
                case 3: 
                    GetComponent<Image>().color = new Color(1f, 0.95f, 0.42f);
                    break;
                case 4: 
                    GetComponent<Image>().color = new Color(0.5f, 0.95f, 1f);
                    break;
                default:
                    GetComponent<Image>().color = new Color(0.6f, 0.6f, 0.9f);
                    break;
            }
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        tierDescription.SetActive(true);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        tierDescription.SetActive(false);
    }
}