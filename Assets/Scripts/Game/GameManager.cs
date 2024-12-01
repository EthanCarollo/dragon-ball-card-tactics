
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
    
    public BoardObject[,] boardCharacterArray;
    public List<Card> PlayerCards = new List<Card>();
    public int CurrentMana = 1;
    public int MaxMana = 1;

    private GameManager()
    {
        Cursor.SetCursor(SpriteDatabase.Instance.normalCursor, Vector2.zero, CursorMode.Auto);
        boardCharacterArray = new BoardObject[BoardWidth, BoardHeight];
        GoNextFight();
        PlayerCards = CardDatabase.Instance.playerCards.ToList();
        SetupCard();
    }

    public void GoNextFight()
    {
        Fight randomFight = FightDatabase.Instance.GetRandomFight();
        foreach (var characterContainerFight in randomFight.opponents)
        {
            boardCharacterArray[characterContainerFight.position.x, characterContainerFight.position.y] 
                = new BoardCharacter(new CharacterContainer(characterContainerFight.characterData.id), false);
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