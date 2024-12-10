using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropRateBox : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler {
    public CardRarity rarity;
    public TextMeshProUGUI text;
    public Image image;

    private GameObject annotationInstance;

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject annotationPrefab = PrefabDatabase.Instance.annotationUiPrefab;
        annotationInstance = Instantiate(annotationPrefab, transform);
        annotationInstance.GetComponentInChildren<TextMeshProUGUI>().text = "Card drop rate for " + rarity.ToString().FirstCharacterToUpper() + " rarity";
        annotationInstance.transform.position = Input.mousePosition;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (annotationInstance != null)
        {
            Destroy(annotationInstance);
            annotationInstance = null;
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (annotationInstance != null)
        {
            annotationInstance.transform.position = Input.mousePosition;
        }
    }

    public void SetupBox(){
        text.text = new CardDropRate(GameManager.Instance.Player.Level.CurrentLevel).GetRarityPercentage(rarity).ToString() + "%";
        image.color = rarity.GetRarityColor();
    }

    
}