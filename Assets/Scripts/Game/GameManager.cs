
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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
    public PlayerInfo Player = new PlayerInfo();

    public int actualRound = 0;

    private float _difficultyMutliplicator;
    public float difficultyMutliplicator
    {
        get => (float)Math.Round(_difficultyMutliplicator, 2);
        set => _difficultyMutliplicator = value;
    }

    private GameManager()
    {
        difficultyMutliplicator = 1.00f;
        try {
            Cursor.SetCursor(SpriteDatabase.Instance.normalCursor, Vector2.zero, CursorMode.Auto);
        } catch(Exception error){
            Debug.LogWarning("Cannot set cursor for weird reason, " + error.ToString());
        }
        boardCharacterArray = new BoardObject[BoardWidth, BoardHeight];
        PlayerCards = CardDatabase.Instance.playerCards.ToList();
        SetupCard();
        GoNextFight();
    }

    public void GoNextFight()
    {
        Fight randomFight = FightDatabase.Instance.GetRandomFight();
        actualRound ++;
        if(actualRound > 1){
            difficultyMutliplicator += 0.02f;
        }
        BoardGameUiManager.Instance.SetupRoundText(actualRound.ToString());
        foreach (var characterContainerFight in randomFight.opponents)
        {
            boardCharacterArray[characterContainerFight.position.x, characterContainerFight.position.y] 
                = new BoardCharacter(new CharacterContainer(characterContainerFight.characterData.id, difficultyMutliplicator), false);
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

    public List<BoardCharacter> GetCharactersOnBoard()
    {
        List<BoardCharacter> boardCharacters = new List<BoardCharacter>();
        for (int x = 0; x < boardCharacterArray.GetLength(0); x++)
        {
            for (int y = 0; y < boardCharacterArray.GetLength(1); y++)
            {
                if(boardCharacterArray[x, y] is BoardCharacter boardCharacter){
                    boardCharacters.Add(boardCharacter);
                }
            }
        }
        return boardCharacters;
    }

    public void ResetCharacterShader()
    {
        foreach (var boardObj in boardCharacterArray)
        {
            if (boardObj != null && boardObj is BoardCharacter boardCharacter)
            {
                boardCharacter.ResetCharacterShader();
            }
        }
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