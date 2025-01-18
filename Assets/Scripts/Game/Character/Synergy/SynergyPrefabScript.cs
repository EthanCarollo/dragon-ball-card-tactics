using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

public class SynergyPrefabScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public Synergy synergy;
    public TextMeshProUGUI synergyNumber;
    public GameObject tierDescription;
    public TextMeshProUGUI descriptionText;
    public Image synergyImage;

    public Transform characterContainer;
    public GameObject simpleCharacterContainer;

    public void Setup(Synergy synergy) {
        tierDescription.SetActive(false);
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

        foreach (Transform child in characterContainer)
        {
            Destroy(child.gameObject);
        }

        var (boardCharactersWithSynergy, databaseCharactersWithSynergy) = synergy.GetCharactersWithSynergy();

        List<int> alreadyCreatedSynergy = new List<int>();
        foreach (var bc in boardCharactersWithSynergy)
        {
            alreadyCreatedSynergy.Add(bc.character.GetCharacterData().id);
            Instantiate(simpleCharacterContainer, characterContainer).GetComponent<Image>().sprite = bc.character.GetCharacterData().characterIcon;
        }
        foreach (var charWithSynergy in databaseCharactersWithSynergy)
        {
            if(alreadyCreatedSynergy.Contains(charWithSynergy.id)) continue;
            var imageCharContainer = Instantiate(simpleCharacterContainer, characterContainer).GetComponent<Image>();
            imageCharContainer.color = new Color(0.4f, 0.4f, 0.4f);
            imageCharContainer.sprite = charWithSynergy.characterIcon;
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