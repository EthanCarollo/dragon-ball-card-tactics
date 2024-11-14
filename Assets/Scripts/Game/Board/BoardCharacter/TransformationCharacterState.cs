using UnityEngine;

public class TransformationCharacterState : BoardCharacterState
{
    private TransformAnimation transformAnimation;
    
    public TransformationCharacterState(BoardCharacter character, TransformAnimation animation) : base(character)
    {
        transformAnimation=animation;
        boardCharacter.PlayAnimation(transformAnimation);
    }
    
    public override void Update()
    {
        
    }

    
    public override void Attack(int damage, Particle particle)
    {
        Debug.LogWarning("Launch attack if default character");
    }
    
    public override bool CanKikoha(){
        return false;
    }
    
    public override void Transform(TransformAnimation animation){
        
    }
}