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

    public virtual void LaunchKikoha()
    {
        boardCharacter.UpdateState(new ChargingCharacterState(boardCharacter));
    }
    public virtual void UpdateKikohaAdvancement(int percentage){

    }
    public virtual void EndKikoha(){

    }
    public virtual int GetKikohaAdvancement(){
        return 0;
    }
}
