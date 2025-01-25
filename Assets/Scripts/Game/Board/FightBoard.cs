using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FightBoard : Board
{
    public static FightBoard Instance;
    public GameObject tilePrefab;  
    public BoardState state;
    public Transform fightObjectContainer;
    public Transform boardObjectContainer;
    float _tileWidth = 1.0f;
    float _tileHeight = 1.0f;

    void Start()
    {
        Instance = this;
        state = new DefaultBoardState(this);
        // state = new FightBoardState(this);
        BoardArray = new GameObject[GameManager.BoardWidth, GameManager.BoardHeight];
        BoardGameUiManager.Instance.RefreshUI();
        CreateBoard(GameManager.Instance.boardCharacterArray);
    }

    private void Update()
    {
        state.Update();
    }

    public void UpdateState(BoardState newState)
    {
        state = newState;
        newState.Start();
    }

    public void LaunchFight()
    {
        state.LaunchFight();
    }

    public bool IsFighting()
    {
        return state.IsFighting();
    }

    public void EndFight(bool win)
    {
        state.EndFight(win);
    }
    
    public override void CreateBoard(BoardObject[,] boardCharacterArray)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int x = 0; x < GameManager.BoardWidth; x++)
        {
            for (int y = 0; y < GameManager.BoardHeight; y++)
            {
                float posX = x * _tileWidth;
                float posY = y * _tileHeight;
                Vector3 position = new Vector3(posX, posY, 0);
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, this.transform);
                var tileScript = tile.AddComponent<TileBehaviour>();
                tileScript.assignedBoard = this;
                tileScript.position = new Vector2Int(x, y);
                tile.transform.localScale = new Vector3(1f, 1f, 1);
                tile.name = $"Tile {x},{y}";
                BoardArray[x, y] = tile;
            }
        }
        InitializeCharacter(boardCharacterArray);
    }
    
    private Dictionary<(BoardCharacter, Vector2Int), GameObject> instantiatedCharacters = new();

    private void InitializeCharacter(BoardObject[,] characters)
    {
        HashSet<(BoardCharacter, Vector2Int)> activeKeys = new();
        
        for (int x = 0; x < characters.GetLength(0); x++)
        {
            for (int y = 0; y < characters.GetLength(1); y++)
            {
                var boardObject = characters[x, y];
                if (boardObject == null) continue;
                
                float posX = x * _tileWidth;
                float posY = y * _tileHeight;
                Vector3 position = new Vector3(posX, posY, 0);
                try {
                    if (boardObject is BoardCharacter character)
                    {
                        if (character.character.IsDead()) continue;
                        
                        var key = (character, new Vector2Int(x, y));
                        activeKeys.Add(key);
                        if (instantiatedCharacters.TryGetValue(key, out GameObject existingGameObject)) {
                            if (existingGameObject == null) // Reinstantiate if destroyed
                                instantiatedCharacters[key] = InstantiateCharacter(character, position);
                        }
                        else {
                            instantiatedCharacters[key] = InstantiateCharacter(character, position);
                        }
                    }
                }
                catch (Exception e) {
                    Debug.LogError(e);
                    if (boardObject is BoardCharacter character)
                    {
                        Debug.Log("Exception when instantiating game object of character : " + character.character.GetName());
                    }
                    else
                    {
                        Debug.Log("Exception when instantiating game object of board object");
                    }
                }
            }
        }
        
        foreach (var key in new List<(BoardCharacter, Vector2Int)>(instantiatedCharacters.Keys))
        {
            if (!activeKeys.Contains(key) && instantiatedCharacters[key] != null)
            {
                Destroy(instantiatedCharacters[key]);
                instantiatedCharacters.Remove(key);
            }
        }
    }
    
    private GameObject InstantiateCharacter(BoardCharacter character, Vector3 position)
    {
        GameObject characterGameObject = Instantiate(character.character.GetCharacterData().characterPrefab, position, Quaternion.identity, boardObjectContainer);

        // Configure GameObject
        var charPrefabScript = characterGameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>();
        charPrefabScript.assignedBoard = this;
        charPrefabScript.boardCharacter = character;
        charPrefabScript.position = new Vector2Int((int)(position.x / _tileWidth), (int)(position.y / _tileHeight));
        charPrefabScript.spriteSocle.color = new Color(
            character.character.isPlayerCharacter ? 0f : 1f, 0f, character.character.isPlayerCharacter ? 1f : 0f, 0.2f);
        charPrefabScript.spriteRenderer.sprite = character.character.GetCharacterData().characterSprite;
        charPrefabScript.spriteRenderer.sortingOrder = 4;
        charPrefabScript.spriteRenderer.flipX = !character.character.isPlayerCharacter;

        character.SetGameObject(characterGameObject);
        character.SetBoard(this);
        character.SetCharacterSlider();
        character.PlayAnimation(SpriteDatabase.Instance.appearAnimation);
        
        if (GlobalGameConfig.Instance.debug)
        {
            Debug.Log($"Instantiate character at position: {position} | Character: {character} | Name: {character.character.GetName()} | ID: {character.character.GetCharacterData().id}");
        }

        return characterGameObject;
    }

    public override bool AddCharacterFromBoard(BoardCharacter character, Vector2Int position)
    {
        GameManager.Instance.boardCharacterArray[position.x, position.y] = character;
        return true;
    }
    
    public float size = 1f;

    public void LaunchCinematic()
    {
        state.LaunchCinematic();
    }

    public void EndCinematic()
    {
        state.EndCinematic();
    }
}
