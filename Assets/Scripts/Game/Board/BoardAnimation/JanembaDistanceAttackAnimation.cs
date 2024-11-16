using UnityEngine;
using System.Collections;
using System;


[CreateAssetMenu(fileName = "New Janemba Distance Attack Animation", menuName = "BoardAnimation/JanembaDistanceAttackAnimation")]
public class JanembaDistanceAttackAnimation : BoardAnimation {
    public int attackFrameIndex;
    public FrameSprite[] handPortalAnimation;
    public int launchProjectHandIndex;
    public Sprite projectile;
    public Particle particleAttack;
    public AttackType attackType;
    public Vector2 startMargin;

    public override IEnumerator PlayAnimationCoroutine(BoardCharacter character)
    {
        character.isAnimating = true;
        var index = 0;
        
        foreach (FrameSprite frameSprite in frameSprites)
        {
            character.gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().spriteRenderer.sprite = frameSprite.sprite;
            yield return new WaitForSeconds(frameSprite.time); 
            if(index == attackFrameIndex){
                GameObject handObject = new GameObject("JanembaPortalHand");
                SpriteRenderer spriteRenderer = handObject.AddComponent<SpriteRenderer>();
                handObject.transform.position = character.GetCharacterTarget().gameObject.transform.position + new Vector3(character.direction.normalized.x * 2f, 0.6f, 0);
                handObject.transform.rotation = Quaternion.Euler(0, 0, 180 * character.direction.normalized.x);
                if (character.direction.normalized.x < 0)
                {
                    handObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                spriteRenderer.sortingOrder = 5; // Temp
                
                var handIndex = 0;
                foreach (FrameSprite handFrameSprite in handPortalAnimation)
                {
                    spriteRenderer.sprite = handFrameSprite.sprite;
                    yield return new WaitForSeconds(handFrameSprite.time);
                    if (handIndex == launchProjectHandIndex)
                    {
                        LaunchAttack(character, handObject.transform.position);
                    }
                    handIndex++;
                }
                
                Destroy(handObject);

            }
            index++;
        }
        character.isAnimating = false;
    }

    private void LaunchAttack(BoardCharacter character, Vector3 fromPosition){
        
        GameObject newGameObject = new GameObject("Projectile");
        SpriteRenderer spriteRenderer = newGameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = projectile;
        spriteRenderer.sortingOrder = 6;
        newGameObject.transform.position = fromPosition + new Vector3(-character.direction.normalized.x, 0, 0);
        
        Vector3 direction = character.direction.normalized;
        float angle = Mathf.Atan2(direction.y, -direction.x) * Mathf.Rad2Deg;
        newGameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        
        try
        {
            LeanTween.move(newGameObject, character.GetCharacterTarget().gameObject.transform.position + new Vector3(0, 0.5f), 0.1f)
            .setOnComplete(() => {
                switch (attackType)
                {
                    case AttackType.Normal :
                        character.Attack(particleAttack);
                        break;
                    case AttackType.Critical :
                        character.CriticalAttack(particleAttack);
                        break;
                    case AttackType.Special :
                        character.SpecialAttack(particleAttack);
                        break;
                }
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