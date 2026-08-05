using System;
using TMPro;
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
        var prefabDatabase = PrefabDatabase.Instance;
        if (prefabDatabase == null || prefabDatabase.annotationUiPrefab == null)
        {
            return;
        }

        GameObject annotationPrefab = prefabDatabase.annotationUiPrefab;
        annotationInstance = Instantiate(annotationPrefab, transform);
        var annotationText = annotationInstance.GetComponentInChildren<TextMeshProUGUI>();
        if (annotationText != null)
        {
            var rarityName = rarity.ToString();
            var formattedRarityName = rarityName.Length == 0
                ? rarityName
                : char.ToUpperInvariant(rarityName[0]) + rarityName.Substring(1);
            annotationText.text = "Card drop rate for " + formattedRarityName + " rarity";
        }
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
        if (GameManager.Instance == null)
        {
            return;
        }

        if (text != null)
        {
            text.text = Math.Round(new CardDropRate(GameManager.Instance.Player.Level.CurrentLevel).GetRarityPercentage(rarity), 1).ToString() + "%";
        }
        if (image != null)
        {
            image.color = rarity.GetRarityColor();
        }
    }

    
}
