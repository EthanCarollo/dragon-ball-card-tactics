using UnityEngine;
using System.Collections;
using System;


[Serializable]
public class AttackAnimation : BoardAnimation
{
    public int attackFrameIndex;
    public AttackType attackType;
    public Particle particleAttack;

    public override IEnumerator PlayAnimationCoroutine(BoardCharacter character)
    {
        character.isAnimating = true;
        var index = 0;
        foreach (FrameSprite frameSprite in frameSprites)
        {
            character.gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().spriteRenderer.sprite = frameSprite.sprite;
            yield return new WaitForSeconds(frameSprite.time); 
            if(index == attackFrameIndex){
                switch (attackType)
                {
                    case AttackType.Normal :
                        character.Attack(particleAttack);
                        break;
                    case AttackType.Critical :
                        character.CriticalAttack(particleAttack);
                        break;
                    case AttackType.Special :
                        character.SpecialAttack(particleAttack);
                        break;
                }
            }
            index++;
        }
        character.isAnimating = false;
    }
}