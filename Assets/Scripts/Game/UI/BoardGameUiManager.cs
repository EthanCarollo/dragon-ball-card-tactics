using UnityEngine;

public class BoardGameUiManager : MonoBehaviour
{
    public static BoardGameUiManager Instance;
    public CharacterBoardUi characterBoardUi;
    public GameObject playCardScreen;
    public GameObject draggedCardPrefab;
    
    public void Awake()
    {
        Instance = this;
    }

    public void ShowPlayCardPanel()
    {
        playCardScreen.SetActive(true);
    }

    public void HidePlayCardPanel()
    {
        playCardScreen.SetActive(false);
    }
}
