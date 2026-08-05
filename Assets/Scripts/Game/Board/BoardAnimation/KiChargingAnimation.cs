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

    public override string GetDescription(CharacterContainer character)
    {
        return $"Focuses energy and charges up, gaining <color=#007ACC>{kiGived}</color> ki.";
    }

    public override string GetDetailledDescription(CharacterContainer character)
    {
        return $"Focuses energy and charges up, gaining <color=#007ACC>{kiGived}</color> ki.";
    }

    public override Sprite GetIcon(){
        if(animationIcon != null) return base.GetIcon();
        else return SpriteDatabase.Instance.kiChargingAbilityIcon;
    }

    public override IEnumerator PlayAnimationCoroutine(BoardCharacter character)
    {
        character.actualAnimation = this;
        yield return new WaitForSeconds(0.5f);
        var index = 0;
        var spriteRenderer = GetCharacterSpriteRenderer(character);
        foreach (FrameSprite frameSprite in GetValidFrames())
        {
            if (spriteRenderer != null) spriteRenderer.sprite = frameSprite.sprite;
            yield return new WaitForSeconds(GetFrameDuration(frameSprite));
            if(index == attackFrameIndex){
                character.AddKi(kiGived);
            }
            index++;
        }
        EndAnimation(character);
    }
}
