
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
    public float elapsedTime = 0f;
    
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

    public Fight ActualFight;

    private GameManager()
    {
        _instance = this;
        boardCharacterArray = new BoardObject[BoardWidth, BoardHeight];
    }

    // This function should be called on start scene
    public void Start(){
        CharacterDatabase.Instance.AssignUniqueIDs();
        SetMap(PrefabDatabase.Instance.namekDefaultMap);
        difficultyMutliplicator = 1.00f;
        elapsedTime = 0f;
        actualRound = 0;
        try {
            Cursor.SetCursor(SpriteDatabase.Instance.normalCursor, Vector2.zero, CursorMode.Auto);
        } catch(Exception error){
            Debug.LogWarning("Cannot set cursor for weird reason, " + error.ToString());
        }
        boardCharacterArray = new BoardObject[BoardWidth, BoardHeight];
        PlayerCards = CardDatabase.Instance.playerCards.ToList();
        try {
            SetupCard();
        } catch(Exception error){
            Debug.LogError(error);
        }
        try {
            GoNextFight();
        } catch(Exception error){
            Debug.LogError(error);
        }
    }

    public GameObject actualMap;

    public void SetMap(GameObject map){
        if(actualMap != null){
            MonoBehaviour.Destroy(actualMap);
        }
        actualMap = MonoBehaviour.Instantiate(map);
        actualMap.transform.position = new Vector3(0, 0.3f, 0);
    }

    public void GoNextFight()
    {
        FightDifficulty difficulty;
        if (actualRound % 12 == 0 && actualRound != 0)
        {
            difficulty = FightDifficulty.Hardcore;
        }
        else if (actualRound % 6 == 0 && actualRound != 0)
        {
            difficulty = FightDifficulty.Hard;
        }
        else if (actualRound % 3 == 0 && actualRound != 0)
        {
            difficulty = FightDifficulty.Medium;
        }
        else
        {
            difficulty = FightDifficulty.Easy;
        }
        ActualFight = FightDatabase.Instance.GetRandomFight(difficulty);
        BoardGameUiManager.Instance.fightNameUi.OpenFightNamePanel(ActualFight);
        actualRound ++;
        if(actualRound > 1){
            difficultyMutliplicator += 0.05f;
        }
        Debug.LogWarning("Chosed fight is : " + ActualFight.name);
        BoardGameUiManager.Instance.SetupRoundText(actualRound);
        CleanGameBoard();
        if(ActualFight.map != null){
            SetMap(ActualFight.map);
        }
        foreach (var characterContainerFight in ActualFight.opponents)
        {
            if(CharacterDatabase.Instance.GetCharacterById(characterContainerFight.characterData.id).characterName != characterContainerFight.characterData.characterName){
                Debug.LogWarning("Weird behaviour, ID of character in fight : " + ActualFight.name + " is not the same than the real id");
                Debug.Log(characterContainerFight.characterData.id);
                Debug.Log(characterContainerFight.characterData.characterName);
            };
            boardCharacterArray[characterContainerFight.position.x, characterContainerFight.position.y] 
                = new BoardCharacter(new CharacterContainer(characterContainerFight.characterData.id, new List<CharacterPassive>(), 1, false, difficultyMutliplicator));    
        }
        FightBoard.Instance.CreateBoard(boardCharacterArray);
    }

    public void SetupCard()
    {
        CardUi.Instance.SetupCardUi(PlayerCards);
    }

    public void AddCard(Card card)
    {
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
                if (boardCharacterArray[x, y] is BoardCharacter boardCharacter && boardCharacter.character.isPlayerCharacter)
                {
                    continue;
                }
                boardCharacterArray[x, y] = null;
            }
        }
    }

    public List<Synergy> GetActiveSynergy(bool playerSynergy = true){
        List<Synergy> ingameSynergy = new List<Synergy>();
        foreach (var boardCharacter in GetCharactersOnBoard())
        {
            var synergies = boardCharacter.character.GetSynergies();
            if(boardCharacter.character.isPlayerCharacter == playerSynergy && synergies != null){
                foreach (var synergy in synergies)
                {
                    if(ingameSynergy.Contains(synergy)) continue;
                    ingameSynergy.Add(synergy);
                }
            }
        }
        return ingameSynergy;
    }
}