using UnityEngine;

public class BoardGameUiManager : MonoBehaviour
{
    public static BoardGameUiManager Instance;
    public CharacterBoardUi characterBoardUi;
    
    public void Awake()
    {
        Instance = this;
    }
}
