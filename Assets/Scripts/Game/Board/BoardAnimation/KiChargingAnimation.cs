using UnityEngine;
using System.Collections;
using System;



[CreateAssetMenu(fileName = "New Ki Charging Animation", menuName = "BoardAnimation/KiChargingAnimation")]
public class KiChargingAnimation : BoardAnimation
{
    public int attackFrameIndex;
    public AttackType attackType;
    public Particle particleAttack;
    public int kiGived;

    public override IEnumerator PlayAnimationCoroutine(BoardCharacter character)
    {
        character.actualAnimation = this;
        yield return new WaitForSeconds(0.5f);
        var index = 0;
        foreach (FrameSprite frameSprite in frameSprites)
        {
            character.gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().spriteRenderer.sprite = frameSprite.sprite;
            yield return new WaitForSeconds(frameSprite.time); 
            if(index == attackFrameIndex){
                character.AddKi(kiGived);
            }
            index++;
        }
        EndAnimation(character);
    }
}