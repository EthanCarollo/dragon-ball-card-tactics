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
    
    public abstract void Update();
}
