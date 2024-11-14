using UnityEngine;

public abstract class BoardObject
{
    public GameObject gameObject;
    public Board board;

    public BoardObject SetGameObject(GameObject gameObject)
    {
        this.gameObject = gameObject;
        return this;
    }

    public BoardObject SetBoard(Board board)
    {
        this.board = board;
        return this;
    }
    
    public abstract void UpdateUi();
    public abstract void Update();
    public abstract BoardObject Clone();

    public void RemoveFromBoard()
    {
        GameObject.Destroy(gameObject);
        if (this.board is FightBoard fightBoard)
        {
            for (int x = 0; x < GameManager.Instance.boardCharacterArray.GetLength(0); x++)
            {
                for (int y = 0; y < GameManager.Instance.boardCharacterArray.GetLength(1); y++)
                {
                    BoardObject boardObject = GameManager.Instance.boardCharacterArray[x, y];
                    if (boardObject == this) GameManager.Instance.boardCharacterArray[x, y] = null;
                }
            }
        }
    }
}
