using UnityEngine;
using System.Collections;
using System;
using UnityEditor.UIElements;

[Serializable]
public class BoardAnimation {
    [SerializeField]
    public FrameSprite[] frameSprites;

    public virtual IEnumerator PlayAnimationCoroutine(BoardCharacter character)
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

public enum AttackType
{
    Normal,
    Critical,
    Special,
}