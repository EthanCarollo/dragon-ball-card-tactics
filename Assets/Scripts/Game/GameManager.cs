
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
    public CharacterInventory characterInventory;
    public CampaignManager campaignManager;
    public int CurrentMana = 1;
    public int MaxMana = 1;

    private GameManager()
    {
        characterInventory = new CharacterInventory();
        if (DebugMode)
        {
            characterInventory.characters.Add(new CharacterContainer(1));
            characterInventory.characters.Add(new CharacterContainer(2));
        }
        campaignManager = new CampaignManager();
        
        boardCharacterArray = new BoardObject[BoardWidth, BoardHeight];
        // TODO : add a global config value for the board object outside of the fight board "the reserve"
        // TODO : cause actually it's the value in vertical board lol
        boardUsableCharacterArray = new BoardObject[8]; 
        
        /*
        // TODO : temporary
        if (DebugMode)
        // if(false)
        {
            boardCharacterArray[1, 4] = new BoardCharacter(Resources.Load<CharacterData>("ScriptableObjects/Gohan"), true);
            //boardCharacterArray[0, 2] = new BoardCharacter(Resources.Load<CharacterData>("ScriptableObjects/Paragus"), true);
            //boardUsableCharacterArray[0] = new BoardCharacter(Resources.Load<CharacterData>("ScriptableObjects/BlackGoku"), true);
            boardCharacterArray[7, 4] = new BoardCharacter(Resources.Load<CharacterData>("ScriptableObjects/BlackGoku"), false);
        }
        */
    }

    public void SetupGameBoardForLevel(Level level, CharacterContainer[] playerCharacter)
    {
        foreach (var child in level.characters)
        {
            boardCharacterArray[child.position.x, child.position.y] = new BoardCharacter(new CharacterContainer(child.character.id), false);
        }

        for (int i = 0; i < playerCharacter.Length; i++)
        {
            boardUsableCharacterArray[i] = new BoardCharacter(playerCharacter[i], true);
        }
    }
}