
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public bool needOnBoarding = true;

    public CampaignContainer actualCampaign;
    public int actualCampaignLevel = 0;

    public List<Card> PlayerCards = new List<Card>();
    public int CurrentMana = 1;
    public int MaxMana = 1;

    private GameManager()
    {
        Cursor.SetCursor(SpriteDatabase.Instance.normalCursor, Vector2.zero, CursorMode.Auto);
        SetupGalaxy(GalaxyDatabase.Instance.galaxies[0]);
        characterInventory = CharacterInventory.Instance;
        campaignManager = new CampaignManager();
        
        boardCharacterArray = new BoardObject[BoardWidth, BoardHeight];
        // TODO : add a global config value for the board object outside of the fight board "the reserve"
        // TODO : cause actually it's the value in vertical board lol
        boardUsableCharacterArray = new BoardObject[8]; 
    }

    public void SetupGalaxy(Galaxy galaxy)
    {
        actualGalaxy = galaxy;
        foreach (var actualGalaxyCampaign in actualGalaxy.campaigns)
        {
            actualGalaxyCampaign.actualCampaign = 0;
        }
    }

    public void SetupCard()
    {
        CardUi.Instance.SetupCardUi(PlayerCards);
    }

    public void AddCard(Card card){
        PlayerCards.Add(card);
        SetupCard();
    }

    public void RemoveCard(Card card)
    {
        PlayerCards.Remove(card);
        SetupCard();
    }

    public void SetupCampaign(CampaignContainer campaign, CharacterContainer[] charContainerForFight){
        actualCampaignLevel = 0;
        actualCampaign = campaign;
        foreach (var characterContainer in charContainerForFight)
        {
            AddCard(characterContainer.GetCharacterData().card);
        }
        PlayerCards = charContainerForFight.Select((characterContainer) =>
            {
                characterContainer.GetCharacterData().card.name = characterContainer.GetCharacterData().name;
                characterContainer.GetCharacterData().card.image =
                    characterContainer.GetCharacterData().characterSprite;
                characterContainer.GetCharacterData().card.character =
                    characterContainer;
                return characterContainer.GetCharacterData().card;
            }).Cast<Card>().ToList();
        SetupCard();
        SetupGameBoardForLevel(actualCampaign.GetActualCampaign().levels[actualCampaignLevel]);
        FightBoard.Instance.CreateBoard();
        VerticalBoard.Instance.CreateBoard();
    }

    public void GoNextLevel(){
        actualCampaignLevel++;
        if (actualCampaignLevel > actualCampaign.GetActualCampaign().levels.Length - 1)
        {
            actualCampaign.GetActualCampaign().EndCampaign();
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

    public void SetupGameBoardForLevel(Level level)
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
            var characterContainer = new CharacterContainer(child.character.id);
            characterContainer.unlockedPassives = child.unlockPassive;
            boardCharacterArray[child.position.x, child.position.y] = new BoardCharacter(characterContainer, false);
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