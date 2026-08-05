using UnityEngine;
using System.Collections;
using System;
using System.Linq;


[CreateAssetMenu(fileName = "New Apply All Effect Animation", menuName = "BoardAnimation/ApplyAllEffectAnimation")]
public class ApplyAllEffectAnimation : BoardAnimation {
    public int attackFrameIndex;
    
    public Effect[] effectApplied;

    public override Sprite GetIcon(){
        if(animationIcon != null) return base.GetIcon();
        else return SpriteDatabase.Instance.applyAllEffectAbilityIcon;
    }

    public override string GetDescription(CharacterContainer character)
    {
        if (effectApplied == null || effectApplied.Length == 0)
            return "Applies no effect.";

        string[] effectNames = effectApplied
            .Where(effect => effect != null)
            .Select(effect => effect.effectName)
            .Distinct()
            .ToArray();

        string effectsList = string.Join(", ", effectNames);

        return $"Applies the following effects to all <color=#007ACC>allies</color>: <b>{effectsList}</b>.";
    }


    public override IEnumerator PlayAnimationCoroutine(BoardCharacter character)
    {
        character.actualAnimation = this;
        var index = 0;
        var spriteRenderer = GetCharacterSpriteRenderer(character);
        foreach (FrameSprite frameSprite in GetValidFrames())
        {
            if (spriteRenderer != null) spriteRenderer.sprite = frameSprite.sprite;
            yield return new WaitForSeconds(GetFrameDuration(frameSprite));
            if(index == attackFrameIndex){
                var charactersToAffect = GameManager.Instance.GetCharactersOnBoard().Where(charac => charac.character.isPlayerCharacter == character.character.isPlayerCharacter);
                foreach (var characterToAffect in charactersToAffect)
                {
                    foreach (var effect in effectApplied ?? Array.Empty<Effect>())
                    {
                        if (effect != null) characterToAffect.character.AddEffect(effect);
                    }
                }
            }
            index++;
        }
        EndAnimation(character);
    }
}
