using UnityEngine;
using System.Collections;
using System;


[Serializable]
public class DistanceAttackAnimation : BoardAnimation {
    public int attackFrameIndex;
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
                LaunchAttack(character);
            }
            index++;
        }
        character.isAnimating = false;
    }

    private void LaunchAttack(BoardCharacter character){
        GameObject newGameObject = new GameObject("Projectile");
        SpriteRenderer spriteRenderer = newGameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = projectile;
        spriteRenderer.sortingOrder = 3;
        newGameObject.transform.position = character.gameObject.transform.position + new Vector3((startMargin.x * character.direction.x), startMargin.y);
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
            Debug.LogWarning("Error on moving game object of projectile, so delete it.");
            MonoBehaviour.Destroy(newGameObject);
        }
    }
}