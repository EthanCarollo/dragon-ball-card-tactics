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
    public virtual void LaunchKikohaFight(BoardObject boardObject1, BoardObject boardObject2){
        this.board.UpdateState(new KikohaFightBoardState(board, boardObject1, boardObject2));
    }
    public virtual void EndKikohaFight()
    {
        
    }
    public abstract void EndFight();

}
