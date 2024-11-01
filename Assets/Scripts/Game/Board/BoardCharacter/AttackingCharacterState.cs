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
        if (characterTarget != null && characterTarget.character.IsDead() == false && characterTarget.gameObject != null)
        {
            boardCharacter.direction = BoardUtils.GetDirectionVector(
                characterTarget.gameObject.transform.position - boardCharacter.gameObject.transform.position); 
        }
        
        if (boardCharacter.character.IsDead())
        {
            return;
        }

        if (characterTarget.character.IsDead())
        {
            boardCharacter.UpdateState(new DefaultCharacterState(boardCharacter));
            return;
        }
        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack >= (1 / boardCharacter.character.GetAttackSpeed()))
        {
            if (boardCharacter.character.actualKi >= boardCharacter.character.GetCharacterData().maxKi)
            {
                boardCharacter.PlayAnimation(boardCharacter.character.GetCharacterData().specialAttackAnimation);
                boardCharacter.character.actualKi = 0;
            }
            else
            {
                if(IsCritical(boardCharacter.character.GetCriticalChance()) == true)
                {
                    boardCharacter.PlayAnimation(boardCharacter.character.GetCharacterData().criticalAttackAnimation);
                    boardCharacter.AddKi(15);    
                } else {
                    boardCharacter.PlayAnimation(boardCharacter.character.GetCharacterData().attackAnimation);
                    boardCharacter.AddKi(15);
                }
            }
            timeSinceLastAttack = 0f;
        }
        boardCharacter.PlayAnimationIfNotRunning(boardCharacter.character.GetCharacterData().idleAnimation);
        boardCharacter.SetCharacterSlider();
    }
    
    public override void Attack(int damage, Particle particle = null)
    {
        if (boardCharacter.character.IsDead())
        {
            return;
        }
        if (particle != null) {
            particle.StartParticle(characterTarget.gameObject.transform.position);
        }
        characterTarget.HitDamage(boardCharacter.character.GetCharacterData().baseDamage);
    }

    static bool IsCritical(int chance)
    {
        System.Random random = new System.Random();
        int randomValue = random.Next(0, 100); 
        return randomValue < chance; 
    }
}
