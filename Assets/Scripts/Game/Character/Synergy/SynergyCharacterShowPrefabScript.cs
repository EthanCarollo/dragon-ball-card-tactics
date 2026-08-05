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
        if (synergyDescriptionContainer == null)
        {
            return;
        }

        synergyDescriptionContainer.gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(synergyDescriptionContainer);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (synergyDescriptionContainer == null)
        {
            return;
        }

        synergyDescriptionContainer.gameObject.SetActive(false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(synergyDescriptionContainer);
    }

    public void Setup(Synergy synergy) {
        if (synergy == null)
        {
            return;
        }

        this.synergy = synergy;
        if (synergyNumber != null) synergyNumber.text = synergy.synergyName;
        if (synergyImage != null) synergyImage.sprite = synergy.synergyImage;
        if (synergyDescriptionText != null)
        {
            synergyDescriptionText.text = synergy.GetDescription();
            synergyDescriptionText.maskable = false;
        }
        if (synergyDescriptionContainer != null)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(synergyDescriptionContainer);
            synergyDescriptionContainer.gameObject.SetActive(false);
        }
    }

}
