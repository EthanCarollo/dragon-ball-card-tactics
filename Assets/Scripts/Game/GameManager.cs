
using UnityEngine;

public class GameManager
{
    private static GameManager _instance;
    public static bool DebugMode = true;
    public static int BoardWidth = 11;
    public static int BoardHeight = 7;
    
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }
    
    public BoardCharacter[,] boardCharacterArray;

    private GameManager()
    {
        boardCharacterArray = new BoardCharacter[BoardWidth, BoardHeight];
        if (DebugMode)
        {
            boardCharacterArray[0, 0] = new BoardCharacter(Resources.Load<CharacterData>("ScriptableObjects/Freezer"));
            boardCharacterArray[0, 1] = new BoardCharacter(Resources.Load<CharacterData>("ScriptableObjects/Freezer"));
        }
    }
}