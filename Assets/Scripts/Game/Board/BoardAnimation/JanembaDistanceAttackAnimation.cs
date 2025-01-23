using UnityEngine;
using System.Collections;
using System;
using UnityEditor.PackageManager;


[CreateAssetMenu(fileName = "New Janemba Distance Attack Animation", menuName = "BoardAnimation/JanembaDistanceAttackAnimation")]
public class JanembaDistanceAttackAnimation : BoardAnimation {
    public int attackFrameIndex;
    public FrameSprite[] handPortalAnimation;
    public int launchProjectHandIndex;
    public Sprite projectile;
    public Particle particleAttack;
    public AttackType attackType;

    public override IEnumerator PlayAnimationCoroutine(BoardCharacter character)
    {
        character.actualAnimation = this;
        var target = character.GetCharacterTarget();
        var index = 0;
        
        foreach (FrameSprite frameSprite in frameSprites)
        {
            character.gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().spriteRenderer.sprite = frameSprite.sprite;
            yield return new WaitForSeconds(frameSprite.time); 
            if(index == attackFrameIndex){
                LaunchHandAnimation(character, target);
            }
            index++;
        }
        EndAnimation(character);
    }

    private void LaunchHandAnimation(BoardCharacter character, BoardCharacter target){
        FightBoard.Instance.StartCoroutine(HandAnimation(character, target));
    }

    private IEnumerator HandAnimation(BoardCharacter character, BoardCharacter target){
        GameObject handObject = new GameObject("JanembaPortalHand");
        try {
            handObject.transform.SetParent(FightBoard.Instance.fightObjectContainer);
        } catch(Exception error){
            Debug.LogWarning("Cannot set hand object in fight board object container" + error.ToString());
        }
        SpriteRenderer spriteRenderer = handObject.AddComponent<SpriteRenderer>();
        handObject.transform.position = character.GetCharacterTarget().gameObject.transform.position + new Vector3(character.direction.normalized.x * 2f, 0.6f, 0);
        handObject.transform.rotation = Quaternion.Euler(0, 0, 180 * character.direction.normalized.x);
        if (character.direction.normalized.x < 0)
        {
            handObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        spriteRenderer.sortingOrder = 5; 
        
        var handIndex = 0;
        foreach (FrameSprite handFrameSprite in handPortalAnimation)
        {
            spriteRenderer.sprite = handFrameSprite.sprite;
            yield return new WaitForSeconds(handFrameSprite.time);
            if (handIndex == launchProjectHandIndex)
            {
                LaunchAttack(character, handObject.transform.position, target);
            }
            handIndex++;
        }
        
        Destroy(handObject);

    }

    private void LaunchAttack(BoardCharacter character, Vector3 fromPosition, BoardCharacter target){
        
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
                        character.Attack(1, particleAttack, target);
                        break;
                    case AttackType.Critical :
                        character.Attack(2, particleAttack, target);
                        break;
                    case AttackType.Special :
                        character.Attack(4, particleAttack, target);
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