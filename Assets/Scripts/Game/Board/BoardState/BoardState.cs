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
    public abstract void EndFight(bool win);
    
    public abstract void LaunchCinematic(BoardCharacter boardChar);
    public abstract void EndCinematic();

    
    // Useful
    protected void ResetAllPassives()
    {
        for (int x = 0; x < GameManager.Instance.boardCharacterArray.GetLength(0); x++)
        {
            for (int y = 0; y < GameManager.Instance.boardCharacterArray.GetLength(1); y++)
            {
                var character = GameManager.Instance.boardCharacterArray[x, y];
                if (character == null) continue;

                if (character is BoardCharacter boardChar && boardChar.character.GetCharacterPassives() is not null)
                {
                    foreach (var passive in boardChar.character.GetCharacterPassives())
                    {
                        if(passive is not null) passive.Setup(boardChar);
                    }
                }
            }
        }
    }
}
