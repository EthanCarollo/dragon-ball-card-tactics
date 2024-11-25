public abstract class BoardFightState
{
    protected FightBoardState boardFightState;
    
    public BoardFightState(FightBoardState boardFightState)
    {
        this.boardFightState = boardFightState;  
    }

    public abstract void Update();

    public abstract void LaunchCinematic();
    public abstract void EndCinematic();
}