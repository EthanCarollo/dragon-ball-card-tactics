using UnityEngine;
using System.Collections;
using System;



[CreateAssetMenu(fileName = "New Attack Animation", menuName = "BoardAnimation/AttackAnimation")]
public class AttackAnimation : BoardAnimation
{
    public int attackFrameIndex;
    public AttackType attackType;
    public Particle particleAttack;
    public int kiOnAttack = 10;
    public bool isCinematic = false;
    
    [SerializeReference, SubclassSelector]
    public Effect[] effectApplied;

    public override IEnumerator PlayAnimationCoroutine(BoardCharacter character)
    {
        var target = character.GetCharacterTarget();
        if (character.board is FightBoard fightBoard && isCinematic)
        {
            fightBoard.LaunchCinematic(character);
            CameraScript.Instance.SetupCameraOnTarget(4.5f, character.gameObject.transform);
        }
        
        character.isAnimating = true;
        var index = 0;
        foreach (FrameSprite frameSprite in frameSprites)
        {
            character.gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().spriteRenderer.sprite = frameSprite.sprite;
            yield return new WaitForSeconds(frameSprite.time); 
            if(index == attackFrameIndex){
                if(effectApplied != null && effectApplied.Length > 0)
                {
                    foreach (var effect in effectApplied)
                    {
                        target.character.AddEffect(effect);
                    }
                }
                character.AddKi(kiOnAttack);
                switch (attackType)
                {
                    case AttackType.Normal :
                        character.Attack(1, particleAttack, target);
                        break;
                    case AttackType.Critical :
                        character.Attack(2, particleAttack, target);
                        break;
                    case AttackType.Special :
                        character.Attack(4, particleAttack, target);
                        break;
                }
            }
            index++;
        }
        character.isAnimating = false;
        
        if (character.board is FightBoard fightBoard2 && isCinematic)
        {
            fightBoard2.EndCinematic();
            CameraScript.Instance.SetupNormalCamera();
        }
    }
}