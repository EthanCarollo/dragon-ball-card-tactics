using UnityEngine;
using System.Collections;
using System;



[CreateAssetMenu(fileName = "New Transform Animation", menuName = "BoardAnimation/TransformAnimation")]
public class TransformAnimation : BoardAnimation {
    public bool resetToBaseCharacter;
    public CharacterData newCharacterData;

    public virtual bool CanTransform(BoardCharacter character)
    {
        return true;
    }

    public override Sprite GetIcon(){
        if(animationIcon != null) return base.GetIcon();
        else return SpriteDatabase.Instance.transformAbilityIcon;
    }

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

        if(resetToBaseCharacter && character.character.GetCharacterData().baseCharacter != null){
            yield return SpriteDatabase.Instance.basicTransformAnimation.PlayAnimationCoroutineTransform(character);
        }

        yield return PlayAnimationCoroutineTransform(character);
        character.SetupCharacter(newCharacterData);
        character.Heal(character.character.GetCharacterMaxHealth());
        
        CameraScript.Instance.SetupNormalCamera();
        yield return new WaitForEndOfFrame();
        EndAnimation(character);
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
        var index = 0;
        foreach (FrameSprite frameSprite in frameSprites)
        {
            character.gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().spriteRenderer.sprite = frameSprite.sprite;
            yield return new WaitForSeconds(frameSprite.time);
            index++;
        }
    }
}