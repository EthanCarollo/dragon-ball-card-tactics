using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SynergyCharacterShowPrefabScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public Synergy synergy;
    public TextMeshProUGUI synergyNumber;
    public Image synergyImage;
    public RectTransform synergyDescriptionContainer;
    public TextMeshProUGUI synergyDescriptionText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        synergyDescriptionContainer.gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(synergyDescriptionContainer);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        synergyDescriptionContainer.gameObject.SetActive(false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(synergyDescriptionContainer);
    }

    public void Setup(Synergy synergy) {
        this.synergy = synergy;
        synergyNumber.text = synergy.synergyName;
        synergyImage.sprite = synergy.synergyImage;
        synergyDescriptionText.text = synergy.GetDescription();
        synergyDescriptionText.maskable = false;
        LayoutRebuilder.ForceRebuildLayoutImmediate(synergyDescriptionContainer);
        synergyDescriptionContainer.gameObject.SetActive(false);
    }

}