using System;
using UnityEngine;
using UnityEngine.UI;

public class PanelSwitcher : MonoBehaviour
{
    public RectTransform content;
    public static PanelSwitcher Instance;
    private int currentIndex = 0;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        if (GameManager.Instance.needOnBoarding)
        {
            DialogManager.Instance.SetupDialog(DialogDatabase.Instance.startDialogs);
        }
    }

    public void SwitchToPanel(int index)
    {
        currentIndex = index;
        float panelWidth = content.rect.width;
        float targetPositionX = -panelWidth * index;
        
        LeanTween.cancel(content);
        LeanTween.moveX(content, targetPositionX, 0.4f).setEase(LeanTweenType.easeInOutCubic);
    }
}