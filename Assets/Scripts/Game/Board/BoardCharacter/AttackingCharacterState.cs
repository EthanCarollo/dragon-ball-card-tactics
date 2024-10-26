using System;
using UnityEngine;

public class AttackingCharacterState : BoardCharacterState
{
    BoardCharacter characterTarget;
    private float timeSinceLastAttack = 0f;
    private Animator animator;
    
    public AttackingCharacterState(BoardCharacter character, BoardCharacter characterTarget) : base(character)
    {
        this.characterTarget = characterTarget;
        //this.animator = boardCharacter.gameObject.transform.GetChild(0).GetComponent<Animator>();
    }
    
    public override void Update()
    {
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
                boardCharacter.PlayAnimation(boardCharacter.character.superAttackSprites);
                boardCharacter.actualKi = 0;
            }
            else
            {
                if(IsCritical(boardCharacter.GetCriticalChance()) == true)
                {
                    boardCharacter.PlayAnimation(boardCharacter.character.criticalAttackSprites);
                    boardCharacter.AddKi(15);    
                } else {
                    boardCharacter.PlayAnimation(boardCharacter.character.attackSprites);
                    boardCharacter.AddKi(15);
                }
            }
            timeSinceLastAttack = 0f;
        }
        boardCharacter.PlayAnimationIfNotRunning(boardCharacter.character.idleSprites);
        boardCharacter.SetCharacterSlider();
    }
    
    public override void Attack(int damage, GameObject particle)
    {
        if (boardCharacter.IsDead())
        {
            return;
        }
        ParticleManager.Instance.InstantiateParticle(characterTarget.gameObject.transform.position, ParticleData.Instance.sparkParticlePrefab);
        characterTarget.HitDamage(boardCharacter.character.baseDamage);
    }

    static bool IsCritical(int chance)
    {
        System.Random random = new System.Random();
        int randomValue = random.Next(0, 100); 
        return randomValue < chance; 
    }
}
