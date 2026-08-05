using UnityEngine;

public abstract class BoardObject
{
    public GameObject gameObject;
    public Board board;
    public bool isInstantiated = false;

    public BoardObject SetGameObject(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.isInstantiated = true;
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
        if (gameObject != null)
        {
            GameObject.Destroy(gameObject);
        }

        ReleaseBoardPosition();
    }

    public void ReleaseBoardPosition()
    {
        if (board is not FightBoard || GameManager.Instance.boardCharacterArray == null)
        {
            return;
        }

        for (int x = 0; x < GameManager.Instance.boardCharacterArray.GetLength(0); x++)
        {
            for (int y = 0; y < GameManager.Instance.boardCharacterArray.GetLength(1); y++)
            {
                if (GameManager.Instance.boardCharacterArray[x, y] == this)
                {
                    GameManager.Instance.boardCharacterArray[x, y] = null;
                }
            }
        }
    }
}
