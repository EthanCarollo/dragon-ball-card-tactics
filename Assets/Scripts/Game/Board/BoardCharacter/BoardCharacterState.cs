using UnityEngine;

public abstract class BoardCharacterState
{
    protected BoardCharacter boardCharacter;
    
    public BoardCharacterState(BoardCharacter boardCharacter)
    {
        this.boardCharacter = boardCharacter;  
    }
    
    public abstract void Update();
    public abstract void Attack(int damage, Particle particle = null);

    public virtual void Dead()
    {
        boardCharacter.UpdateState(new DeadCharacterState(boardCharacter));
    }
}
