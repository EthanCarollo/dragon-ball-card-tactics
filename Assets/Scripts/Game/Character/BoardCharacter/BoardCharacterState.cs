public abstract class BoardCharacterState
{
    BoardCharacter _boardCharacter;
    
    public BoardCharacterState(BoardCharacter boardCharacter)
    {
        this._boardCharacter = boardCharacter;    
    }
    
    public abstract void Update();
}
