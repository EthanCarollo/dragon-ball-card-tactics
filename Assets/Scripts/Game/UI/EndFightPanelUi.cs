using UnityEngine;
using UnityEngine.UI;

public class EndFightPanelUi : MonoBehaviour
{
    public static EndFightPanelUi Instance;
    public GameObject EndFightPanel;
    public Button EndFightButton;
    
    public void Awake()
    {
        Instance = this;
    }

    public void SetupEndFightPanel()
    {
        EndFightPanel.SetActive(true);
    }

    public void CloseEndFightPanel()
    {
        EndFightPanel.SetActive(false);
    }
    
}
