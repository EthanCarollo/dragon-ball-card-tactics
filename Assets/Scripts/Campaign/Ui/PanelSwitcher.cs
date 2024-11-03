using UnityEngine;
using UnityEngine.UI;

public class PanelSwitcher : MonoBehaviour
{
    public RectTransform content;
    private int currentIndex = 0;

    public void SwitchToPanel(int index)
    {
        currentIndex = index;
        float panelWidth = content.rect.width;
        float targetPositionX = -panelWidth * index;
        
        content.anchoredPosition = new Vector2(targetPositionX, content.anchoredPosition.y);
    }
}