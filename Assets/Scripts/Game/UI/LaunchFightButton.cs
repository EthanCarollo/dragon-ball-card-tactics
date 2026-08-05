using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LaunchFightButton : MonoBehaviour, IPointerClickHandler
{
    public Button button;
    private bool CanLaunchFight()
    {
        if (GameManager.Instance == null)
        {
            return false;
        }

        return GameManager.Instance.GetCharactersOnBoard()
            .Any(character => character?.character != null && character.character.isPlayerCharacter);
    }
    
    public void Update()
    {
        if (CanLaunchFight())
        {
            if (button != null)
            {
                button.interactable = true;
            }
        }
        else if (button != null)
        {
            button.interactable = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (CanLaunchFight())
        {
            FightBoard.Instance?.LaunchFight();
        }
    }
}
