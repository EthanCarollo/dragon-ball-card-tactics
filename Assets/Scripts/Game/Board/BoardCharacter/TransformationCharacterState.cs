using UnityEngine;

public class TransformationCharacterState : BoardCharacterState
{
    private TransformAnimation transformAnimation;
    private bool transformed = false;
    public TransformationCharacterState(BoardCharacter character, TransformAnimation animation) : base(character)
    {
        transformAnimation=animation;
    }
    
    public override void Update()
    {
        if (transformed == false)
        {
            transformed = boardCharacter.PlayAnimationIfNotRunning(transformAnimation);
        }
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