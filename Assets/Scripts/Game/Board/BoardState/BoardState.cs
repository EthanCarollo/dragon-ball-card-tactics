public abstract class BoardState
{
    protected Board board;
    
    public BoardState(Board board)
    {
        this.board = board;  
    }
    
    public abstract void Update();
    public abstract void LaunchFight();
}
