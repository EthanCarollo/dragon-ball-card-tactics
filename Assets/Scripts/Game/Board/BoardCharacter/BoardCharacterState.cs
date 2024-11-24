using UnityEngine;

public abstract class BoardCharacterState
{
    protected BoardCharacter boardCharacter;
    
    public BoardCharacterState(BoardCharacter boardCharacter)
    {
        this.boardCharacter = boardCharacter;  
    }
    
    public abstract void Update();


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
    public virtual bool CanKikoha(){
        return true;
    }
    public virtual void EndKikoha(){

    }
    public virtual int GetKikohaAdvancement(){
        return 0;
    }
    public virtual void Transform(TransformAnimation animation){
        boardCharacter.UpdateState(new TransformationCharacterState(boardCharacter, animation));
    }
}
