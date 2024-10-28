
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
    
    public BoardObject[] boardUsableCharacterArray;
    public BoardObject[,] boardCharacterArray;
    public int CurrentMana = 1;
    public int MaxMana = 1;

    private GameManager()
    {
        boardCharacterArray = new BoardObject[BoardWidth, BoardHeight];
        // TODO : add a global config value for the board object outside of the fight board "the reserve"
        // TODO : cause actually it's the value in vertical board lol
        boardUsableCharacterArray = new BoardObject[8]; 
        if (DebugMode)
        {
            boardCharacterArray[1, 5] = new BoardCharacter(Resources.Load<CharacterData>("ScriptableObjects/BlackGoku"), true);
            boardCharacterArray[0, 2] = new BoardCharacter(Resources.Load<CharacterData>("ScriptableObjects/Paragus"), true);
            boardUsableCharacterArray[0] = new BoardCharacter(Resources.Load<CharacterData>("ScriptableObjects/BlackGoku"), true);
            boardCharacterArray[7, 4] = new BoardCharacter(Resources.Load<CharacterData>("ScriptableObjects/BlackGoku"), false);
        }
    }
}