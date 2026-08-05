using UnityEngine;
using System.Collections;
using System;
using System.Security.Cryptography;


[CreateAssetMenu(fileName = "New Basic Transform Animation", menuName = "BoardAnimation/BasicTransformAnimation")]
public class BasicTransformAnimation : BoardAnimation
{
    private GameObject transformObject;
    
    public override IEnumerator PlayAnimationCoroutine(BoardCharacter character)
    {
        character.actualAnimation = this;
        // var target = character.GetCharacterTarget().gameObject.transform;
        if (character.board is FightBoard fightBoard)
        {
            fightBoard.LaunchCinematic();
        }
        yield return new WaitForEndOfFrame();
        CameraScript.Instance.SetupCameraOnTarget(2.5f, character.gameObject.transform);

        yield return PlayAnimationCoroutineTransform(character);
        character.Heal(character.character.GetCharacterMaxHealth());
        
        CameraScript.Instance.SetupNormalCamera();
        yield return new WaitForEndOfFrame();
        EndAnimation(character);
    }

    public override Sprite GetIcon(){
        if(animationIcon != null) return base.GetIcon();
        else return SpriteDatabase.Instance.transformAbilityIcon;
    }

    public override void EndAnimation(BoardCharacter character)
    {
        base.EndAnimation(character);
        
        if (character.board is FightBoard fightBoard)
        {
            fightBoard.EndCinematic();
            if (!fightBoard.IsFighting())
            {
                CameraScript.Instance.SetupNormalCamera();
            }
        }
    }

    public IEnumerator PlayAnimationCoroutineTransform(BoardCharacter character)
    {
        yield return new WaitForSeconds(0.3f);
        transformObject = new GameObject();
        transformObject.transform.SetParent(character.gameObject.transform);
        transformObject.transform.localPosition = Vector3.zero;
        var spriteRender = transformObject.AddComponent<SpriteRenderer>();
        spriteRender.spriteSortPoint = SpriteSortPoint.Pivot;
        var characterSpriteRenderer = GetCharacterSpriteRenderer(character);
        spriteRender.sortingOrder = characterSpriteRenderer == null ? 0 : characterSpriteRenderer.sortingOrder;
        var index = 0;
        foreach (FrameSprite frameSprite in GetValidFrames())
        {
            spriteRender.sprite = frameSprite.sprite;
            yield return new WaitForSeconds(GetFrameDuration(frameSprite));
            if (index == 2)
            {
                var currentData = character.character?.GetCharacterData();
                var baseCharacter = currentData?.baseCharacter;
                var idleFrames = baseCharacter?.idleAnimation?.frameSprites;
                if (characterSpriteRenderer != null && idleFrames != null && idleFrames.Length > 0)
                {
                    characterSpriteRenderer.sprite = idleFrames[0]?.sprite;
                }
                if (baseCharacter != null) character.SetupCharacter(baseCharacter);
            }
            index++;
        }
        Destroy(transformObject.gameObject);
        yield return new WaitForSeconds(0.3f);
    }
}
