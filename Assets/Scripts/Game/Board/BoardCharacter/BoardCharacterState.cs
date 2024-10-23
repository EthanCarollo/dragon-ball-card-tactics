using UnityEngine;

public abstract class BoardCharacterState
{
    protected BoardCharacter boardCharacter;
    
    public BoardCharacterState(BoardCharacter boardCharacter)
    {
        this.boardCharacter = boardCharacter;  
    }
    
    public abstract void Update();
}
