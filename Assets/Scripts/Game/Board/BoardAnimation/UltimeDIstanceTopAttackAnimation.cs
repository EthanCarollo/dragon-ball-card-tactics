using UnityEngine;
using System.Collections;
using System;


[CreateAssetMenu(fileName = "New Ultime Distance Top Attack Animation", menuName = "BoardAnimation/UltimeDistanceTopAttackAnimation")]
public class UltimeDistanceTopAttackAnimation : BoardAnimation {
    public FrameSprite flySprite;
    public int attackFrameIndex;
    public Sprite projectile;
    public Particle particleAttack;
    public AttackType attackType;
    public Vector2 startMargin;

    public override IEnumerator PlayAnimationCoroutine(BoardCharacter character)
    {
        CameraScript.Instance.SetupCameraOnTarget(4.5f, character.gameObject.transform);
        var target = character.GetCharacterTarget();
        if (character.board is FightBoard fightBoard)
        {
            fightBoard.LaunchCinematic();
        }
        character.actualAnimation = this;
        var startPos = character.gameObject.transform.position;
        character.gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().spriteRenderer.sprite = flySprite.sprite;
        LeanTween.move(character.gameObject, character.gameObject.transform.position + new Vector3(-1f, 2.5f, 0f), 0.8f).setEase(LeanTweenType.easeInOutQuad);
        yield return new WaitForSeconds(0.8f);
        
        var index = 0;
        foreach (FrameSprite frameSprite in frameSprites)
        {
            character.gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().spriteRenderer.sprite = frameSprite.sprite;
            yield return new WaitForSeconds(frameSprite.time); 
            if(index == attackFrameIndex){
                LaunchAttack(character, target);
            }
            index++;
        }
        
        
        character.gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().spriteRenderer.sprite = flySprite.sprite;
        LeanTween.move(character.gameObject, startPos, 0.8f).setEase(LeanTweenType.easeInOutQuad);
        yield return new WaitForSeconds(0.8f);
        
        EndAnimation(character);
    }

    public override void EndAnimation(BoardCharacter character)
    {
        base.EndAnimation(character);
        
        if (character.board is FightBoard fightBoard2)
        {
            fightBoard2.EndCinematic();
        }
        CameraScript.Instance.SetupFightCamera();
    }

    private void LaunchAttack(BoardCharacter character, BoardCharacter target){
        GameObject newGameObject = new GameObject("Projectile");
        SpriteRenderer spriteRenderer = newGameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = projectile;
        spriteRenderer.sortingOrder = 4;
        newGameObject.transform.position = character.gameObject.transform.position + new Vector3((startMargin.x * character.direction.x), startMargin.y);
        
        Vector3 direction = character.direction.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        newGameObject.transform.rotation = Quaternion.Euler(0, 0, angle);
        
        try
        {
            LeanTween.move(newGameObject, target.gameObject.transform.position + new Vector3(0, 0.5f), 0.15f)
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