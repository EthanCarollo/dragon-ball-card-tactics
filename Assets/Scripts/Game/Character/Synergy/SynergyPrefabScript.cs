using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;
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
        if (synergy == null)
        {
            return;
        }

        tierDescription?.SetActive(false);
        this.synergy = synergy;
        if (synergyNumber != null) synergyNumber.text = synergy.GetActiveUnit(true).ToString();
        if (descriptionText != null) descriptionText.text = synergy.GetDescription();
        if (synergyImage != null) synergyImage.sprite = synergy.synergyImage;
        var tierBonuses = synergy.GetActiveTierBonuses(true);
        var image = GetComponent<Image>();
        if(tierBonuses.Count == 0){
            if (image != null) image.color = new Color(0.4f, 0.4f, 0.4f);
            if (transform.childCount > 0)
            {
                var childImage = transform.GetChild(0).GetComponent<Image>();
                if (childImage != null) childImage.color = new Color(0.4f, 0.4f, 0.4f);
            }
        } else {
            Color tierColor;
            switch(tierBonuses.Count){
                case 1: 
                    tierColor = new Color(0.5f, 0.4f, 0.3f);
                    break;
                case 2: 
                    tierColor = new Color(1f, 0.9f, 0.95f);
                    break;
                case 3: 
                    tierColor = new Color(1f, 0.95f, 0.42f);
                    break;
                case 4: 
                    tierColor = new Color(0.5f, 0.95f, 1f);
                    break;
                default:
                    tierColor = new Color(0.6f, 0.6f, 0.9f);
                    break;
            }
            if (image != null) image.color = tierColor;
        }

        if (characterContainer == null || simpleCharacterContainer == null)
        {
            return;
        }

        foreach (Transform child in characterContainer)
        {
            Destroy(child.gameObject);
        }

        var (boardCharactersWithSynergy, databaseCharactersWithSynergy) = synergy.GetCharactersWithSynergy();

        List<int> alreadyCreatedSynergy = new List<int>();
        foreach (var bc in boardCharactersWithSynergy ?? new List<BoardCharacter>())
        {
            var characterData = bc?.character?.GetCharacterData();
            if (characterData == null) continue;
            alreadyCreatedSynergy.Add(characterData.id);
            var characterImage = Instantiate(simpleCharacterContainer, characterContainer).GetComponent<Image>();
            if (characterImage != null) characterImage.sprite = characterData.characterIcon;
        }
        foreach (var charWithSynergy in databaseCharactersWithSynergy ?? new List<CharacterData>())
        {
            if (charWithSynergy == null) continue;
            if(alreadyCreatedSynergy.Contains(charWithSynergy.id)) continue;
            var imageCharContainer = Instantiate(simpleCharacterContainer, characterContainer).GetComponent<Image>();
            imageCharContainer.color = new Color(0.4f, 0.4f, 0.4f);
            imageCharContainer.sprite = charWithSynergy.characterIcon;
        }
        var tierRectTransform = tierDescription?.GetComponent<RectTransform>();
        if(tierRectTransform != null && (tierRectTransform.position.y - tierRectTransform.sizeDelta.y) < 0){
            tierRectTransform.position = new Vector2(tierRectTransform.position.x, tierRectTransform.sizeDelta.y + 40);
        }
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        tierDescription?.SetActive(true);
        StartCoroutine(SetTierDescriptionPosition());
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        tierDescription?.SetActive(false);
        StartCoroutine(SetTierDescriptionPosition());
    }

    private IEnumerator SetTierDescriptionPosition(){
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        var tierRectTransform = tierDescription?.GetComponent<RectTransform>();
        if(tierRectTransform != null && (tierRectTransform.position.y - tierRectTransform.sizeDelta.y) < 0){
            tierRectTransform.position = new Vector2(tierRectTransform.position.x, tierRectTransform.sizeDelta.y + 40);
        }
    }
}
