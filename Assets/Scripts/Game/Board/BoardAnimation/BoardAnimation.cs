using UnityEngine;
using System.Collections;
using System;


[CreateAssetMenu(fileName = "New Board Animation", menuName = "BoardAnimation/BoardAnimation")]
public class BoardAnimation : ScriptableObject {
    [SerializeField]
    public FrameSprite[] frameSprites;
    public AudioClip audio;

    public virtual IEnumerator PlayAnimationCoroutine(BoardCharacter character)
    {
        PlaySound(character.gameObject.GetComponentInChildren<AudioSource>());
        character.actualAnimation = this;
        foreach (FrameSprite frameSprite in frameSprites)
        {
            character.gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().spriteRenderer.sprite = frameSprite.sprite;
            yield return new WaitForSeconds(frameSprite.time); 
        }
        EndAnimation(character);
    }

    public virtual void EndAnimation(BoardCharacter character)
    {
        if(character.actualAnimation == this) character.actualAnimation = null;
    }

    private void PlaySound(AudioSource audioSource){
        if(audio != null){
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