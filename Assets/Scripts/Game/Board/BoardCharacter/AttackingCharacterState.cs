using System;
using UnityEngine;

public class AttackingCharacterState : BoardCharacterState
{
    public BoardCharacter characterTarget;
    private float timeSinceLastAttack = 0f;
    
    public AttackingCharacterState(BoardCharacter character, BoardCharacter characterTarget) : base(character)
    {
        this.characterTarget = characterTarget;
    }
    
    public override void Update()
    {
        boardCharacter.direction = BoardUtils.GetDirectionVector(
            characterTarget.gameObject.transform.position - boardCharacter.gameObject.transform.position);
        
        if (boardCharacter.IsDead())
        {
            return;
        }

        if (characterTarget.IsDead())
        {
            boardCharacter.UpdateState(new DefaultCharacterState(boardCharacter));
            return;
        }
        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack >= (1 / boardCharacter.GetAttackSpeed()))
        {
            if (boardCharacter.actualKi >= boardCharacter.character.maxKi)
            {
                boardCharacter.PlayAnimation(boardCharacter.character.specialAttackAnimation);
                boardCharacter.actualKi = 0;
            }
            else
            {
                if(IsCritical(boardCharacter.GetCriticalChance()) == true)
                {
                    boardCharacter.PlayAnimation(boardCharacter.character.criticalAttackAnimation);
                    boardCharacter.AddKi(15);    
                } else {
                    boardCharacter.PlayAnimation(boardCharacter.character.attackAnimation);
                    boardCharacter.AddKi(15);
                }
            }
            timeSinceLastAttack = 0f;
        }
        boardCharacter.PlayAnimationIfNotRunning(boardCharacter.character.idleAnimation);
        boardCharacter.SetCharacterSlider();
    }
    
    public override void Attack(int damage, Particle particle = null)
    {
        if (boardCharacter.IsDead())
        {
            return;
        }
        if (particle != null) {
            particle.StartParticle(characterTarget.gameObject.transform.position);
        }
        characterTarget.HitDamage(boardCharacter.character.baseDamage);
    }

    static bool IsCritical(int chance)
    {
        System.Random random = new System.Random();
        int randomValue = random.Next(0, 100); 
        return randomValue < chance; 
    }
}
