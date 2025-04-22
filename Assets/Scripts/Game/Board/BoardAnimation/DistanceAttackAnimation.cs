using UnityEngine;
using System.Collections;
using System;


[CreateAssetMenu(fileName = "New Distance Attack Animation", menuName = "BoardAnimation/DistanceAttackAnimation")]
public class DistanceAttackAnimation : BoardAnimation {
    public int attackFrameIndex;
    public Sprite projectile;
    public Particle particleAttack;
    public AttackType attackType;
    public Vector2 startMargin;
    public int kiOnAttack = 10;
    public int otherTarget = 0;
    
    public Effect[] effectApplied;

    public override string GetDescription(CharacterContainer character)
    {
        return $"Fires a projectile forward, applying effects on hit and restoring <color=#007ACC>{kiOnAttack}</color> ki. Hits <color=#D60000>{1 + otherTarget}</color> target{(otherTarget > 0 ? "s" : "")}.";
    }


    public override IEnumerator PlayAnimationCoroutine(BoardCharacter character)
    {
        var target = character.GetCharacterTarget();
        
        character.actualAnimation = this;
        var index = 0;
        foreach (FrameSprite frameSprite in frameSprites)
        {
            character.gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().spriteRenderer.sprite = frameSprite.sprite;
            yield return new WaitForSeconds(frameSprite.time); 
            if(index == attackFrameIndex){
                LaunchAttack(character, target);
                if(otherTarget != 0){
                    var characterOnBoard = GameManager.Instance.GetCharactersOnBoard();
                    int otherAttack = 0;
                    foreach (var characterToAttack in characterOnBoard)
                    {
                        if(characterToAttack == target || characterToAttack.character.isPlayerCharacter == character.character.isPlayerCharacter) continue;
                        if(otherAttack >= otherTarget) break;
                        otherAttack++;
                        LaunchAttack(character, characterToAttack);
                    }
                }
            }
            index++;
        }
        EndAnimation(character);
    }

    private void LaunchAttack(BoardCharacter character, BoardCharacter target){
        GameObject newGameObject = new GameObject("Projectile");
        SpriteRenderer spriteRenderer = newGameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = projectile;
        spriteRenderer.sortingOrder = 3;
        newGameObject.transform.position = character.gameObject.transform.position + new Vector3((startMargin.x * character.direction.x), startMargin.y);
        
        Vector3 direction = character.direction.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        newGameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        

        try
        {
            LeanTween.move(newGameObject, target.gameObject.transform.position + new Vector3(0, 0.5f), 0.1f)
            .setOnComplete(() => {
                if(effectApplied != null && effectApplied.Length > 0)
                {
                    foreach (var effect in effectApplied)
                    {
                        target.character.AddEffect(effect);
                    }
                }

                try {
                    var activeBonuses = character.character.GetAllActiveBonuses();
                    foreach (var bonus in activeBonuses)
                    {
                        if(bonus is SpecialCharacterBonus specialCharacterBonus){ 
                            if(specialCharacterBonus.character == character.character.GetCharacterData()){
                                foreach(Effect effect in bonus.effectsOnHit){
                                    target.character.AddEffect(effect);
                                }
                            }
                        } else {
                            foreach(Effect effect in bonus.effectsOnHit){
                                target.character.AddEffect(effect);
                            }
                        }
                    }
                } catch(Exception error){
                    Debug.LogError("Cannot inflict effect , " + error);
                }
                
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
                character.AddKi(kiOnAttack);   
                MonoBehaviour.Destroy(newGameObject);
            });
        }
        catch (Exception error)
        {
            Debug.LogWarning("Error on moving game object of projectile, so delete it." + error.ToString());
            MonoBehaviour.Destroy(newGameObject);
        }
    }
}