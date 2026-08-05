using UnityEngine;

public class DeadCharacterState : BoardCharacterState
{
    public DeadCharacterState(BoardCharacter character) : base(character)
    {
        Dead();
    }

    public override void Dead()
    {
        if (boardCharacter == null || boardCharacter.gameObject == null)
        {
            return;
        }

        this.boardCharacter.isDying = true;
        var characterData = this.boardCharacter.character?.GetCharacterData();
        if (characterData?.deadAnimation != null)
        {
            this.boardCharacter.PlayAnimation(characterData.deadAnimation);
        }
        else
        {
            // Disappear 
            if (boardCharacter.actualAnimation != null) boardCharacter.actualAnimation.EndAnimation(this.boardCharacter);
            
            var characterPrefab = this.boardCharacter.GetCharacterPrefabScript();
            var spriteRenderer = characterPrefab?.spriteRenderer;
            if (spriteRenderer == null || ShadersDatabase.Instance == null ||
                ShadersDatabase.Instance.disappearMaterial == null)
            {
                GameObject.Destroy(this.boardCharacter.gameObject);
                return;
            }
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
}
