using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Board : MonoBehaviour
{
    public GameObject tilePrefab;  
    public GameObject[,] BoardArray;
    public BoardState state;
    float _tileWidth = 1.0f;
    float _tileHeight = 1.0f;

    void Start()
    {
        state = new DefaultBoardState(this);
        BoardArray = new GameObject[GameManager.BoardWidth, GameManager.BoardHeight];
        CreateBoard();
        InitializeCharacter();
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
    
    private void CreateBoard()
    {
        for (int x = 0; x < GameManager.BoardWidth; x++)
        {
            for (int y = 0; y < GameManager.BoardHeight; y++)
            {
                float posX = x * _tileWidth;
                float posY = y * _tileHeight;
                Vector3 position = new Vector3(posX, posY, 0);
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, this.transform);
                tile.name = $"Tile {x},{y}";
                BoardArray[x, y] = tile;
            }
        }
    }

    private void InitializeCharacter()
    {
        for (int x = 0; x < GameManager.Instance.boardCharacterArray.GetLength(0); x++)
        {
            for (int y = 0; y < GameManager.Instance.boardCharacterArray.GetLength(1); y++)
            {
                var boardObject = GameManager.Instance.boardCharacterArray[x, y];
                if (boardObject == null) continue;
                
                float posX = x * _tileWidth;
                float posY = y * _tileHeight + 0.05f;
                Vector3 position = new Vector3(posX, posY, 0);
                try {
                    if (boardObject is BoardCharacter character)
                    {
                        GameObject characterGameObject = Instantiate(character.character.characterPrefab, position, Quaternion.identity, transform);
                        character.SetGameObject(characterGameObject);
                        character.SetBoard(this);
                        var charPrefabScript = characterGameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>();
                        charPrefabScript.boardCharacter = character;
                        charPrefabScript.spriteSocle.color = 
                            new Color(character.isPlayerCharacter? 0f: 1f, 0f, character.isPlayerCharacter? 1f: 0f, 0.2f);
                        charPrefabScript.spriteRenderer.sortingOrder = 1;
                        charPrefabScript.spriteRenderer.flipX = !character.isPlayerCharacter;
                        charPrefabScript.healthSlider.maxValue = character.character.maxHealth;
                        charPrefabScript.healthSlider.value = character.actualHealth;
                    }
                }
                catch (Exception e) {
                    Debug.LogError(e);
                    if (boardObject is BoardCharacter character)
                    {
                        Debug.Log("Exception when instantiating game object of character : " + character.character.characterName);
                    }
                    else
                    {
                        Debug.Log("Exception when instantiating game object of board object");
                    }
                }
            }
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
