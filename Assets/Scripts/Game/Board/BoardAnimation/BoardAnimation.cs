using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


[CreateAssetMenu(fileName = "New Board Animation", menuName = "BoardAnimation/BoardAnimation")]
public class BoardAnimation : ScriptableObject {
    [SerializeField]
    public FrameSprite[] frameSprites;
    public AudioClip audio;
    public Sprite animationIcon;

    public virtual Sprite GetIcon(){
        return animationIcon;
    }

    public virtual string GetDescription(CharacterContainer character){
        return "";
    }

    public virtual string GetDetailledDescription(CharacterContainer character){
        return "";
    }

    public virtual IEnumerator PlayAnimationCoroutine(BoardCharacter character)
    {
        if (character == null || character.gameObject == null)
        {
            yield break;
        }

        PlaySound(character.gameObject.GetComponentInChildren<AudioSource>());
        character.actualAnimation = this;
        var characterPrefab = character.gameObject.GetComponentInChildren<CharacterPrefabScript>();
        if (characterPrefab == null || characterPrefab.spriteRenderer == null)
        {
            EndAnimation(character);
            yield break;
        }

        foreach (FrameSprite frameSprite in frameSprites ?? new FrameSprite[0])
        {
            if (frameSprite == null)
            {
                continue;
            }

            characterPrefab.spriteRenderer.sprite = frameSprite.sprite;
            yield return new WaitForSeconds(Mathf.Max(0f, frameSprite.time));
        }
        EndAnimation(character);
    }

    protected SpriteRenderer GetCharacterSpriteRenderer(BoardCharacter character)
    {
        return character?.GetCharacterPrefabScript()?.spriteRenderer;
    }

    protected IEnumerable<FrameSprite> GetValidFrames()
    {
        foreach (var frameSprite in frameSprites ?? Array.Empty<FrameSprite>())
        {
            if (frameSprite != null)
            {
                yield return frameSprite;
            }
        }
    }

    protected static float GetFrameDuration(FrameSprite frameSprite)
    {
        return Mathf.Max(0f, frameSprite?.time ?? 0f);
    }

    public virtual void EndAnimation(BoardCharacter character)
    {
        if(character.actualAnimation == this) character.actualAnimation = null;
    }

    private void PlaySound(AudioSource audioSource){
        if(audio != null && audioSource != null){
            audioSource.PlayOneShot(audio);
        }
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
