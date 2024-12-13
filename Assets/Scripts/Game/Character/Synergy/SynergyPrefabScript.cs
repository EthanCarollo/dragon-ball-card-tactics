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
        synergyNumber.text = synergy.GetActiveUnit().ToString();
        descriptionText.text = synergy.GetDescription();
        synergyImage.sprite = synergy.synergyImage;
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