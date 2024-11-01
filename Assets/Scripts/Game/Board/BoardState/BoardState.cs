public abstract class BoardState
{
    protected FightBoard board;
    
    public BoardState(FightBoard board)
    {
        this.board = board;  
    }
    
    public virtual bool IsFighting()
    {
        return false;
    }
    
    public abstract void Update();
    public abstract void LaunchFight();
    public abstract void EndFight();

}
