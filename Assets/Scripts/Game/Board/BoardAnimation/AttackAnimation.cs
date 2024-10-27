using UnityEngine;
using System.Collections;
using System;


[Serializable]
public class AttackAnimation : BoardAnimation {
    public int attackFrameIndex;

    public override IEnumerator PlayAnimationCoroutine(BoardCharacter character)
    {
        character.isAnimating = true;
        var index = 0;
        foreach (FrameSprite frameSprite in frameSprites)
        {
            character.gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().spriteRenderer.sprite = frameSprite.sprite;
            yield return new WaitForSeconds(frameSprite.time); 
            if(index == attackFrameIndex){
                character.Attack();
            }
            index++;
        }
        character.isAnimating = false;
    }
}