
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Tilemaps;

public class GameManager
{
    private static GameManager _instance;
    public static bool DebugMode = true;
    public static int BoardWidth = 11;
    public static int BoardHeight = 7;
    public float elapsedTime = 0f;
    public HistoryAction[] historyActions;
    
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
        historyActions = new HistoryAction[0];
        boardCharacterArray = new BoardObject[BoardWidth, BoardHeight];
    }

    // This function should be called on start scene
    public void Start(){
        var characterDatabase = CharacterDatabase.Instance;
        var prefabDatabase = PrefabDatabase.Instance;
        var cardDatabase = CardDatabase.Instance;
        if (characterDatabase == null || prefabDatabase == null || cardDatabase == null)
        {
            Debug.LogError("Cannot start the game: one or more required databases are missing.");
            return;
        }

        characterDatabase.AssignUniqueIDs();
        SetMap(prefabDatabase.namekDefaultMap);
        difficultyMutliplicator = 1.00f;
        elapsedTime = 0f;
        actualRound = 0;
        this.Player.Life.CurrentLife = Player.Life.MaxLife;
        try {
            if (SpriteDatabase.Instance != null)
            {
                Cursor.SetCursor(SpriteDatabase.Instance.normalCursor, Vector2.zero, CursorMode.Auto);
            }
        } catch(Exception error){
            Debug.LogWarning("Cannot set cursor for weird reason, " + error.ToString());
        }
        boardCharacterArray = new BoardObject[BoardWidth, BoardHeight];
        PlayerCards = (cardDatabase.playerCards ?? Array.Empty<Card>())
            .Where(card => card != null)
            .ToList();
        SetupCard();
        
        try {
            GoNextFight();
        } catch(Exception error){
            Debug.LogError(error);
        }
    }

    public GameObject actualMap;
    private GameObject actualMapReference;

    public void SetMap(GameObject map){
        if (map == null)
        {
            Debug.LogError("Cannot set a null map.");
            return;
        }

        if(actualMap != null && actualMapReference != map){
            var oldMap = actualMap;
            var oldMapRenderer = oldMap.transform.childCount == 0
                ? null
                : oldMap.transform.GetChild(0).GetComponent<TilemapRenderer>();
            var shaderDatabase = ShadersDatabase.Instance;
            if (oldMapRenderer == null || shaderDatabase == null || shaderDatabase.disappearMaterial == null)
            {
                DestroyMap(oldMap);
            }
            else
            {
                oldMapRenderer.material = new Material(shaderDatabase.disappearMaterial);
                oldMapRenderer.sortingOrder = 1;
                LeanTween.value(oldMap, f =>
                {
                    if (oldMapRenderer != null)
                    {
                        oldMapRenderer.material.SetFloat("_Fade", f);
                    }
                }, 1f, 0f, 1f)
                .setOnComplete(f => DestroyMap(oldMap));
            }
        }
        if(actualMapReference != map){
            actualMapReference = map;
            actualMap = MonoBehaviour.Instantiate(map);
            actualMap.transform.position = new Vector3(0, 0.3f, 0);
        }
    }

    private static void DestroyMap(GameObject map)
    {
        if (map != null)
        {
            MonoBehaviour.Destroy(map);
        }
    }

    public void AddHistoryAction(HistoryAction historyAction){
        if (historyAction == null)
        {
            return;
        }

        var historyActionsList = historyActions.ToList();
        historyActionsList.Add(historyAction);
        historyActions = historyActionsList.ToArray();
    }

    public void GoNextFight()
    {
        var fightDatabase = FightDatabase.Instance;
        if (fightDatabase == null)
        {
            Debug.LogError("Cannot start the next fight: FightDatabase is missing.");
            return;
        }

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
        ActualFight = fightDatabase.GetRandomFight(difficulty);
        if (ActualFight == null)
        {
            Debug.LogError($"Cannot start the next fight: no fight is configured for difficulty {difficulty}.");
            return;
        }

        if (BoardGameUiManager.Instance != null && BoardGameUiManager.Instance.fightNameUi != null)
        {
            BoardGameUiManager.Instance.fightNameUi.OpenFightNamePanel(ActualFight);
        }

        actualRound ++;
        if(actualRound > 1){
            difficultyMutliplicator += 0.05f;
        }
        Debug.Log("Chosed fight is : " + ActualFight.name);
        BoardGameUiManager.Instance?.SetupRoundText(actualRound);
        CleanGameBoard();
        if(ActualFight.map != null){
            SetMap(ActualFight.map);
        } else {
            var prefabDatabase = PrefabDatabase.Instance;
            if (prefabDatabase != null)
            {
                SetMap(prefabDatabase.namekDefaultMap);
            }
        }
        foreach (var characterContainerFight in ActualFight.opponents ?? Array.Empty<CharacterContainerFight>())
        {
            if (characterContainerFight == null || characterContainerFight.characterData == null)
            {
                Debug.LogWarning($"Fight '{ActualFight.name}' contains an invalid opponent and it was skipped.");
                continue;
            }

            if (characterContainerFight.position.x < 0 ||
                characterContainerFight.position.x >= BoardWidth ||
                characterContainerFight.position.y < 0 ||
                characterContainerFight.position.y >= BoardHeight)
            {
                Debug.LogWarning($"Fight '{ActualFight.name}' contains an opponent outside the board and it was skipped.");
                continue;
            }

            var registeredCharacter = CharacterDatabase.Instance?.GetCharacterById(characterContainerFight.characterData.id);
            if (registeredCharacter == null)
            {
                Debug.LogWarning($"Character ID {characterContainerFight.characterData.id} is not registered; opponent skipped.");
                continue;
            }

            if(registeredCharacter.characterName != characterContainerFight.characterData.characterName){
                Debug.LogWarning("Weird behaviour, ID of character in fight : " + ActualFight.name + " is not the same than the real id");
            };

            if (boardCharacterArray[characterContainerFight.position.x, characterContainerFight.position.y] != null)
            {
                Debug.LogWarning($"Fight '{ActualFight.name}' has overlapping opponents at {characterContainerFight.position}; later opponent skipped.");
                continue;
            }

            boardCharacterArray[characterContainerFight.position.x, characterContainerFight.position.y]
                = new BoardCharacter(new CharacterContainer(characterContainerFight.characterData.id, new List<CharacterPassive>(), 1, false, difficultyMutliplicator));    
        }
        FightBoard.Instance?.CreateBoard(boardCharacterArray);
    }

    public void SetupCard()
    {
        CardUi.Instance?.SetupCardUi(PlayerCards);
    }

    public void AddCard(Card card)
    {
        if (card == null)
        {
            return;
        }

        PlayerCards.Add(card);
        SetupCard();
    }

    public void RemoveCard(Card card)
    {
        if (card == null)
        {
            return;
        }

        PlayerCards.Remove(card);
        SetupCard();
    }

    public List<BoardCharacter> GetCharactersOnBoard()
    {
        List<BoardCharacter> boardCharacters = new List<BoardCharacter>();
        if (boardCharacterArray == null)
        {
            return boardCharacters;
        }

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
            if (boardCharacter?.character == null)
            {
                continue;
            }

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
