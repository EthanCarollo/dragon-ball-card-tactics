using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class BoardAnimation {
    [SerializeField]
    public FrameSprite[] frameSprites;

    public IEnumerator PlayAnimationCoroutine(BoardCharacter character)
    {
        character.isAnimating = true;
        foreach (FrameSprite frameSprite in frameSprites)
        {
            character.gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().spriteRenderer.sprite = frameSprite.sprite;
            yield return new WaitForSeconds(frameSprite.time); 
        }
        character.isAnimating = false;
    }
}

[Serializable]
public class FrameSprite {
    public Sprite sprite;
    public float time;
}