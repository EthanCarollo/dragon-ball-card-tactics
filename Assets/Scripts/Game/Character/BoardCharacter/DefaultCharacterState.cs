
using UnityEngine;

public class DefaultCharacterState : BoardCharacterState
{
    public DefaultCharacterState(BoardCharacter character) : base(character) { }
    
    public override void Update()
    {
        Debug.Log("DefaultCharacterState.Update");
    }
}
