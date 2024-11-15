using UnityEngine;
using System.Collections;
using System;



[CreateAssetMenu(fileName = "New Transform Animation", menuName = "BoardAnimation/TransformAnimation")]
public class TransformAnimation : BoardAnimation {
    public CharacterData newCharacterData;

    public virtual bool CanTransform(BoardCharacter character)
    {
        return true;
    }

    public override IEnumerator PlayAnimationCoroutine(BoardCharacter character)
    {
        character.isAnimating = true;
        yield return PlayAnimationCoroutineTransform(character);
        character.SetupCharacter(newCharacterData);
        character.isAnimating = false;
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