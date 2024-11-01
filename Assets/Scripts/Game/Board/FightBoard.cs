using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class FightBoard : Board
{
    public static FightBoard Instance;
    public GameObject tilePrefab;  
    public GameObject[,] BoardArray;
    public BoardState state;
    float _tileWidth = 1.0f;
    float _tileHeight = 1.0f;

    void Start()
    {
        Instance = this;
        state = new DefaultBoardState(this);
        // state = new FightBoardState(this);
        BoardArray = new GameObject[GameManager.BoardWidth, GameManager.BoardHeight];
        CreateBoard();
    }

    private void Update()
    {
        state.Update();
    }

    public void UpdateState(BoardState newState)
    {
        state = newState;
    }

    public void LaunchFight()
    {
        state.LaunchFight();
    }

    public bool IsFighting()
    {
        return state.IsFighting();
    }

    public void EndFight()
    {
        state.EndFight();
    }
    
    public override void CreateBoard()
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
                tile.name = $"Tile {x},{y}";
                BoardArray[x, y] = tile;
            }
        }
        InitializeCharacter(GameManager.Instance.boardCharacterArray);
    }

    private void InitializeCharacter(BoardObject[,] characters)
    {
        for (int x = 0; x < characters.GetLength(0); x++)
        {
            for (int y = 0; y < characters.GetLength(1); y++)
            {
                var boardObject = characters[x, y];
                if (boardObject == null) continue;
                
                float posX = x * _tileWidth;
                float posY = y * _tileHeight + 0.05f;
                Vector3 position = new Vector3(posX, posY, 0);
                try {
                    if (boardObject is BoardCharacter character)
                    {
                        if (character.character.IsDead()) continue;
                        GameObject characterGameObject = Instantiate(character.character.GetCharacterData().characterPrefab, position, Quaternion.identity, transform);
                        character.SetGameObject(characterGameObject);
                        character.SetBoard(this);
                        var charPrefabScript = characterGameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>();
                        charPrefabScript.assignedBoard = this;
                        charPrefabScript.boardCharacter = character;
                        charPrefabScript.position = new Vector2Int(x, y);
                        charPrefabScript.spriteSocle.color = 
                            new Color(character.isPlayerCharacter? 0f: 1f, 0f, character.isPlayerCharacter? 1f: 0f, 0.2f);
                        charPrefabScript.spriteRenderer.sprite = character.character.GetCharacterData().characterSprite;
                        charPrefabScript.spriteRenderer.sortingOrder = 4;
                        charPrefabScript.spriteRenderer.flipX = !character.isPlayerCharacter;
                        character.SetCharacterSlider();
                    }
                }
                catch (Exception e) {
                    Debug.LogError(e);
                    if (boardObject is BoardCharacter character)
                    {
                        Debug.Log("Exception when instantiating game object of character : " + character.character.GetCharacterData().characterName);
                    }
                    else
                    {
                        Debug.Log("Exception when instantiating game object of board object");
                    }
                }
            }
        }
    }

    public override bool AddCharacterFromBoard(BoardCharacter character, Vector2Int position)
    {
        GameManager.Instance.boardCharacterArray[position.x, position.y] = character;
        return true;
    }

    public override void RemoveCharacterFromBoard(BoardCharacter character)
    {
        bool characterFound = false;

        for (int i = 0; i < GameManager.Instance.boardCharacterArray.GetLength(0); i++)
        {
            for (int j = 0; j < GameManager.Instance.boardCharacterArray.GetLength(1); j++)
            {
                if (GameManager.Instance.boardCharacterArray[i, j] == character)
                {
                    GameManager.Instance.boardCharacterArray[i, j] = null; 
                    characterFound = true;
                    break;
                }
            }
    
            if (characterFound)
            {
                break; 
            }
        }

        if (!characterFound)
        {
            Debug.LogWarning("Character not found on the board.");
        }
    }
    
    public float size = 1f;

    void OnDrawGizmos()
    {
        for (int x = 0; x < GameManager.Instance.boardCharacterArray.GetLength(0); x++)
        {
            for (int y = 0; y < GameManager.Instance.boardCharacterArray.GetLength(1); y++)
            {
                var boardObject = GameManager.Instance.boardCharacterArray[x, y];
                if (boardObject == null) continue;
                if (boardObject is BoardCharacter character)
                {
                    if (character.isPlayerCharacter)
                    {
                        Gizmos.color = new Color(1f, 0f, 0f, 0.3f); 
                    }
                    else
                    {
                        Gizmos.color = new Color(0f, 0f, 1f, 0.3f); 
                    }
                }
                Gizmos.DrawCube(new Vector3(x, y+0.25f, 0), new Vector3(size, size, 0));
            }
        }
    }
}
