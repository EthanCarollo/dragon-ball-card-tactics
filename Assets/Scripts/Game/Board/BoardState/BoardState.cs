public abstract class BoardState
{
    protected FightBoard board;
    
    public BoardState(FightBoard board)
    {
        this.board = board;  
    }
    
    public abstract void Update();
    public abstract void LaunchFight();
}
