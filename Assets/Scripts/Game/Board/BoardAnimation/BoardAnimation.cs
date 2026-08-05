using UnityEngine;
using System.Collections;
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
