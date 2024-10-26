
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
    
    public BoardObject[,] boardCharacterArray;

    private GameManager()
    {
        boardCharacterArray = new BoardObject[BoardWidth, BoardHeight];
        if (DebugMode)
        {
            boardCharacterArray[0, 5] = new BoardCharacter(Resources.Load<CharacterData>("ScriptableObjects/SonGoku"), true);
            boardCharacterArray[7, 4] = new BoardCharacter(Resources.Load<CharacterData>("ScriptableObjects/BlackGoku"), false);
        }
        var astart = new AStarPathfinding(boardCharacterArray);
    }
}