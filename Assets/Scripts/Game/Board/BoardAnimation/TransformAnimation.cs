using UnityEngine;
using System.Collections;
using System;


[Serializable]
public class TransformAnimation : BoardAnimation {
    public CharacterData newCharacterData;

    public override IEnumerator PlayAnimationCoroutine(BoardCharacter character)
    {
        character.isAnimating = true;
        var index = 0;
        foreach (FrameSprite frameSprite in frameSprites)
        {
            character.gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().spriteRenderer.sprite = frameSprite.sprite;
            yield return new WaitForSeconds(frameSprite.time);
            index++;
        }
        // Transform the character into a new character
        character.SetupCharacter(newCharacterData);
        
        character.isAnimating = false;
    }
}