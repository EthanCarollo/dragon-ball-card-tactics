using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LaunchFightButton : MonoBehaviour, IPointerClickHandler
{
    public Button button;
    private bool CanLaunchFight()
    {
        try
        {
            if (GameManager.Instance.GetCharactersOnBoard().Where((character => character.character.isPlayerCharacter)).Count() == 0)
            {
                return false;
            }
            return true;
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
            return false;
        }
    }
    
    public void Update()
    {
        if (CanLaunchFight())
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (CanLaunchFight())
        {
            FightBoard.Instance.LaunchFight();
        }
    }
}