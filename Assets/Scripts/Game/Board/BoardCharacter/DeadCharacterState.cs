using UnityEngine;

public class DeadCharacterState : BoardCharacterState
{
    public DeadCharacterState(BoardCharacter character) : base(character)
    {
        Dead();
    }

    public void Dead()
    {
        
        this.boardCharacter.isDying = true;
        if (this.boardCharacter.character.GetCharacterData().deadAnimation != null)
        {
            this.boardCharacter.PlayAnimation(this.boardCharacter.character.GetCharacterData().deadAnimation);
        }
        else
        {
            // Disappear 
            var spriteRenderer = this.boardCharacter.gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
            spriteRenderer.material = new Material(ShadersDatabase.Instance.disappearMaterial);
            spriteRenderer.material.SetFloat("_Fade", 1f);
            LeanTween.value(this.boardCharacter.gameObject, f =>
                {
                    spriteRenderer.material.SetFloat("_Fade", f);
                }, 1f, 0f, 2f)
                .setOnComplete((o =>
                {
                    GameObject.Destroy(this.boardCharacter.gameObject);
                }));
        }  
    }
    
    public override void Update()
    {
        
    }

    public override bool CanKikoha()
    {
        return false;
    }

    public override void Transform(TransformAnimation animation)
    {

    }

    public override void Attack(int damage, Particle particle)
    {
        
    }
}