using UnityEngine;

public class TransformationCharacterState : BoardCharacterState
{
    public TransformationCharacterState(BoardCharacter character, TransformAnimation animation) : base(character)
    {
        character.PlayAnimation(animation);
    }
    
    public override void Update()
    {
    }

    
    public override void Attack(int damage, Particle particle)
    {
        Debug.LogWarning("Launch attack if default character");
    }
    
    public override void Transform(TransformAnimation animation){
        
    }
}