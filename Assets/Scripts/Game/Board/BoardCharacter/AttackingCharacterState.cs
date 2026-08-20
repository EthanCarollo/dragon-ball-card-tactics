using System;
using UnityEngine;

public class AttackingCharacterState : BoardCharacterState
{
    public BoardCharacter characterTarget;
    private float timeSinceLastAttack = 0f;
    private bool isSpecialAttacking = false;
    private bool canAttack{
        get {
            float attackSpeed = Mathf.Max(0.01f, boardCharacter?.character?.GetAttackSpeed() ?? 0f);
            return timeSinceLastAttack >= (1f / attackSpeed);
        }
    }
    
    public AttackingCharacterState(BoardCharacter character, BoardCharacter characterTarget) : base(character)
    {
        this.characterTarget = characterTarget;
    }

    public override void Update()
    {   
        if (isSpecialAttacking) {
            return;
        }
        if (characterTarget?.character != null && characterTarget.character.IsDead() == false && characterTarget.gameObject != null)
        {
            boardCharacter.direction = BoardUtils.GetDirectionVector(
                characterTarget.gameObject.transform.position - boardCharacter.gameObject.transform.position); 
        }
        
        if (boardCharacter.character.IsDead())
        {
            return;
        }

        if (characterTarget?.character == null || characterTarget.character.IsDead())
        {
            boardCharacter.UpdateState(new DefaultCharacterState(boardCharacter));
            return;
        }
        timeSinceLastAttack += Time.deltaTime;
        // Attack only if the animation is idle or run
        if (canAttack && (boardCharacter.actualAnimation == boardCharacter.character.GetCharacterData().idleAnimation ||
                boardCharacter.actualAnimation == boardCharacter.character.GetCharacterData().runAnimation))
        {
            if (boardCharacter.character.actualKi >= boardCharacter.character.GetCharacterMaxKi())
            {
                var specialAttack = boardCharacter.character.GetCharacterSpecialAttack();
                if (specialAttack?.animation != null && boardCharacter.PlayAnimation(specialAttack.animation, () => {
                    isSpecialAttacking = false;
                }))
                {
                    isSpecialAttacking = true;
                    boardCharacter.character.actualKi = 0;
                }
                else
                {
                    PlayNormalAttack();
                }
            }
            else
            {
                PlayNormalAttack();
            }
            timeSinceLastAttack = 0f;
        }
        boardCharacter.PlayAnimationIfNotRunning(boardCharacter.character.GetCharacterData().idleAnimation);
        boardCharacter.SetCharacterSlider();
    }

    static bool IsCritical(int chance)
    {
        return UnityEngine.Random.Range(0, 100) < Mathf.Clamp(chance, 0, 100);
    }

    private void PlayNormalAttack()
    {
        if (IsCritical(boardCharacter.character.GetCriticalChance()))
        {
            boardCharacter.PlayAnimation(boardCharacter.character.GetCharacterData().criticalAttackAnimation);
        }
        else
        {
            boardCharacter.PlayAnimation(boardCharacter.character.GetCharacterData().attackAnimation);
        }
    }
}
