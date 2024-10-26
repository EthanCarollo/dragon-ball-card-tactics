
using UnityEngine;

public class AttackingCharacterState : BoardCharacterState
{
    BoardCharacter characterTarget;
    private float attackCooldown = 0.75f;   // Example cooldown for attack
    private float timeSinceLastAttack = 0f;
    private Animator animator;
    
    public AttackingCharacterState(BoardCharacter character, BoardCharacter characterTarget) : base(character)
    {
        this.characterTarget = characterTarget;
        this.animator = boardCharacter.gameObject.transform.GetChild(0).GetComponent<Animator>();
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
        if (timeSinceLastAttack >= attackCooldown)
        {
            if (boardCharacter.actualKi >= boardCharacter.character.maxKi)
            {
                animator.SetTrigger("special_attack");
                boardCharacter.actualKi = 0;
            }
            else
            {
                animator.SetTrigger("attack");
                boardCharacter.AddKi(15);
            }
            timeSinceLastAttack = 0f;
        }
        boardCharacter.SetCharacterSlider();
    }
    
    public override void Attack(int damage, GameObject particle)
    {
        if (boardCharacter.IsDead())
        {
            return;
        }
        ParticleManager.Instance.InstantiateParticle(characterTarget.gameObject.transform.position ,ParticleData.Instance.sparkParticlePrefab);
        characterTarget.HitDamage(boardCharacter.character.baseDamage);
    }
}
