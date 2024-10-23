
using UnityEngine;

public class AttackingCharacterState : BoardCharacterState
{
    BoardCharacter characterTarget;
    private float attackCooldown = 1.5f;   // Example cooldown for attack
    private float timeSinceLastAttack = 0f;
    private Animator animator;
    
    public AttackingCharacterState(BoardCharacter character, BoardCharacter characterTarget) : base(character)
    {
        this.characterTarget = characterTarget;
        this.animator = boardCharacter.gameObject.transform.GetChild(0).GetComponent<Animator>();
    }
    
    public override void Update()
    {
        timeSinceLastAttack += Time.deltaTime;
        if (timeSinceLastAttack >= attackCooldown)
        {
            animator.SetTrigger("attack");
            timeSinceLastAttack = 0f;
        }
    }
    
    public override void Attack()
    {
        characterTarget.HitDamage(boardCharacter.character.baseDamage);
    }
}
