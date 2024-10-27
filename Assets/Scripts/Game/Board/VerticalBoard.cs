using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class VerticalBoard : Board {

    public GameObject tilePrefab;  
    public GameObject[] BoardArray;
    float _tileHeight = 1.0f;
    int BoardSize = 9;

    void Start()
    {
        BoardArray = new GameObject[BoardSize];
        CreateBoard();
        InitializeCharacter();
    }
    
    private void CreateBoard()
    {
        for (int i = 0; i < BoardSize; i++)
        {
            float posY = i * _tileHeight;
            Vector3 position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + posY, 0);
            GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, this.transform);
            tile.name = $"Tile vertical {i}";
            BoardArray[i] = tile;
            
        }
    }


    private void InitializeCharacter()
    {
        for (int i = 0; i < GameManager.Instance.boardUsableCharacterArray.GetLength(0); i++)
        {
            BoardObject boardObject = GameManager.Instance.boardUsableCharacterArray[i];
            if (boardObject == null) continue;
            
            float posX = this.gameObject.transform.position.x + 0;
            float posY = this.gameObject.transform.position.y + i * _tileHeight + 0.05f;
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
                    charPrefabScript.spriteRenderer.sprite = character.character.characterSprite;
                    character.SetCharacterSlider();
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