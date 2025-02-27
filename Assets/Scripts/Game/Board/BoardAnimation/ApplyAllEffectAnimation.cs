using UnityEngine;
using System.Collections;
using System;
using System.Linq;


[CreateAssetMenu(fileName = "New Apply All Effect Animation", menuName = "BoardAnimation/ApplyAllEffectAnimation")]
public class ApplyAllEffectAnimation : BoardAnimation {
    public int attackFrameIndex;
    
    public Effect[] effectApplied;

    public override IEnumerator PlayAnimationCoroutine(BoardCharacter character)
    {
        var target = character.GetCharacterTarget();

        character.actualAnimation = this;
        var index = 0;
        foreach (FrameSprite frameSprite in frameSprites)
        {
            character.gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().spriteRenderer.sprite = frameSprite.sprite;
            yield return new WaitForSeconds(frameSprite.time); 
            if(index == attackFrameIndex){
                var charactersToAffect = GameManager.Instance.GetCharactersOnBoard().Where(charac => charac.character.isPlayerCharacter == character.character.isPlayerCharacter);
                foreach (var characterToAffect in charactersToAffect)
                {
                    foreach (var effect in effectApplied)
                    {
                        characterToAffect.character.AddEffect(effect);
                    }
                }
            }
            index++;
        }
        EndAnimation(character);
    }
}