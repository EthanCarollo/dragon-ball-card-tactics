using UnityEngine;

public class Board : MonoBehaviour
{
    private int width = 11; 
    private int height = 7; 
    public GameObject tilePrefab;  
    public GameObject[,] boardArray; 

    void Start()
    {
        boardArray = new GameObject[width, height];
        CreateBoard();
    }

    void CreateBoard()
    {
        float tileWidth = 1.0f;
        float tileHeight = 1.0f;
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float posX = x * tileWidth;
                float posY = y * tileHeight;
                Vector3 position = new Vector3(posX, posY, 0);
                GameObject tile = Instantiate(tilePrefab, position, Quaternion.identity, this.transform);
                tile.name = $"Tile {x},{y}";
                boardArray[x, y] = tile;
            }
        }
    }

}
