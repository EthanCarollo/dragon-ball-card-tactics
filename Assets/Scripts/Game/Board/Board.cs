using System;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject tilePrefab;  
    public GameObject[,] BoardArray; 
    float _tileWidth = 1.0f;
    float _tileHeight = 1.0f;

    void Start()
    {
        BoardArray = new GameObject[GameManager.BoardWidth, GameManager.BoardHeight];
        CreateBoard();
        InitializeCharacter();
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
                var character = GameManager.Instance.boardCharacterArray[x, y];
                if (character == null) continue;
                
                float posX = x * _tileWidth;
                float posY = y * _tileHeight + 0.05f;
                Vector3 position = new Vector3(posX, posY, 0);
                try {
                    GameObject characterGameObject = Instantiate(character.character.characterPrefab, position, Quaternion.identity, transform);
                    character.SetGameObject(characterGameObject);
                    character.SetBoard(this);
                    characterGameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 1;
                }
                catch (Exception e) {
                    Debug.LogError(e);
                    Debug.Log("Exception when instantiating game object of character : " + character.character.characterName);
                }
            }
        }
    }

    private void Update()
    {
        for (int x = 0; x < GameManager.Instance.boardCharacterArray.GetLength(0); x++)
        {
            for (int y = 0; y < GameManager.Instance.boardCharacterArray.GetLength(1); y++)
            {
                var character = GameManager.Instance.boardCharacterArray[x, y];
                if (character == null) continue;
                character.Update();
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
                var character = GameManager.Instance.boardCharacterArray[x, y];
                if (character == null) continue;
                Gizmos.color = new Color(1f, 0f, 0f, 0.3f); 
                Gizmos.DrawCube(new Vector3(x, y+0.25f, 0), new Vector3(size, size, 0));
            }
        }
    }
}
