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
    
    public abstract void Start();
    public abstract void Update();
    public abstract void LaunchFight();
    public abstract void EndFight(bool win);
    
    public abstract void LaunchCinematic();
    public abstract void EndCinematic();

    
    // Useful
    protected void ResetAllPassives()
    {
        var boardCharacters = GameManager.Instance?.boardCharacterArray;
        if (boardCharacters == null)
        {
            return;
        }

        for (int x = 0; x < boardCharacters.GetLength(0); x++)
        {
            for (int y = 0; y < boardCharacters.GetLength(1); y++)
            {
                var character = boardCharacters[x, y];
                if (character == null) continue;

                if (character is BoardCharacter boardChar && boardChar.character != null && boardChar.character.GetCharacterPassives() is not null)
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
