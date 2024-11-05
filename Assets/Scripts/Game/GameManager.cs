
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
    public Galaxy actualGalaxy;

    public CampaignContainer actualCampaign;
    public int actualCampaignLevel = 0;

    public int CurrentMana = 1;
    public int MaxMana = 1;

    private GameManager()
    {
        SetupGalaxy(GalaxyDatabase.Instance.galaxies[0]);
        characterInventory = CharacterInventory.Instance;
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

    public void SetupGalaxy(Galaxy galaxy)
    {
        actualGalaxy = galaxy;
        foreach (var actualGalaxyCampaign in actualGalaxy.campaigns)
        {
            actualGalaxyCampaign.actualCampaign = 0;
        }
    }

    public void SetupCampaign(CampaignContainer campaign){
        actualCampaignLevel = 0;
        actualCampaign = campaign;
        SetupGameBoardForLevel(actualCampaign.GetActualCampaign().levels[actualCampaignLevel], new CharacterContainer[]
            {
                characterInventory.characters[0]
            });
        FightBoard.Instance.CreateBoard();
        VerticalBoard.Instance.CreateBoard();
    }

    public void GoNextLevel(){
        actualCampaignLevel++;
        if (actualCampaignLevel > actualCampaign.GetActualCampaign().levels.Length - 1)
        {
            SceneTransitor.Instance.LoadScene(1, () =>
            {
                actualCampaign.actualCampaign++;
                CleanGameBoard();
            });
            return;
        }
        SetupGameBoardForLevel(actualCampaign.GetActualCampaign().levels[actualCampaignLevel]);
        FightBoard.Instance.CreateBoard();
        VerticalBoard.Instance.CreateBoard();
    }

    public void SetupGameBoardForLevel(Level level, CharacterContainer[] playerCharacter = null)
    {
        // Clean board before going on level.
        for (int x = 0; x < boardCharacterArray.GetLength(0); x++)
        {
            for (int y = 0; y < boardCharacterArray.GetLength(1); y++)
            {
                var boardObject = boardCharacterArray[x, y];
                if (boardObject is BoardCharacter character && character != null && character.character.IsDead())
                {
                    boardCharacterArray[x, y] = null;
                }
            }
        }

        foreach (var child in level.characters)
        {
            boardCharacterArray[child.position.x, child.position.y] = new BoardCharacter(new CharacterContainer(child.character.id), false);
        }

        if(playerCharacter == null) return;
        for (int i = 0; i < playerCharacter.Length; i++)
        {
            boardUsableCharacterArray[i] = new BoardCharacter(playerCharacter[i], true);
        }
        
        DialogManager.Instance.SetupDialog(level.StartDialog);
    }

    private void CleanGameBoard()
    {
        for (int x = 0; x < boardCharacterArray.GetLength(0); x++)
        {
            for (int y = 0; y < boardCharacterArray.GetLength(1); y++)
            {
                boardCharacterArray[x, y] = null;
            }
        }
    }
}