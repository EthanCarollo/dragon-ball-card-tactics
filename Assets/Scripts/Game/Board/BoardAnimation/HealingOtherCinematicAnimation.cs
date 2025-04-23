using UnityEngine;
using System.Collections;
using System;



[CreateAssetMenu(fileName = "New Healing Other Animation", menuName = "BoardAnimation/HealingOtherCinematicAnimation")]
public class HealingOtherCinematicAnimation : BoardAnimation
{
    public int attackFrameIndex;
    public AttackType attackType;
    public Particle particleAttack;

    public override string GetDescription(CharacterContainer character)
    {
        return $"Performs a healing cinematic, restoring <color=#32CD32>20%</color> max health to all allies of the same team using a <color=#6A5ACD>{attackType}</color> healing attack.";
    }

    public override string GetDetailledDescription(CharacterContainer character)
    {
        return $"Performs a healing cinematic, restoring <color=#32CD32>20%</color> max health to all allies of the same team using a <color=#6A5ACD>{attackType}</color> healing attack.";
    }

    public override Sprite GetIcon(){
        if(animationIcon != null) return base.GetIcon();
        else return SpriteDatabase.Instance.healingOtherAbilityIcon;
    }

    public override IEnumerator PlayAnimationCoroutine(BoardCharacter character)
    {
        CameraScript.Instance.SetupCameraOnTarget(5.5f, character.gameObject.transform);
        var target = character.GetCharacterTarget().gameObject.transform;
        if (character.board is FightBoard fightBoard)
        {
            fightBoard.LaunchCinematic();
        }
        character.actualAnimation = this;
        yield return new WaitForSeconds(0.5f);
        var index = 0;
        foreach (FrameSprite frameSprite in frameSprites)
        {
            character.gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().spriteRenderer.sprite = frameSprite.sprite;
            yield return new WaitForSeconds(frameSprite.time); 
            if(index == attackFrameIndex)
            {
                var characters = GameManager.Instance.boardCharacterArray;
                foreach (var boardObject in characters)
                {
                    if (boardObject is BoardCharacter boardCharacter && boardCharacter.character.isPlayerCharacter == character.character.isPlayerCharacter)
                    {
                        boardCharacter.Heal(boardCharacter.character.GetCharacterMaxHealth() / 5);
                    }
                }
            }
            index++;
        }
        EndAnimation(character);
    }

    public override void EndAnimation(BoardCharacter character)
    {
        base.EndAnimation(character);
        
        if (character.board is FightBoard fightBoard2)
        {
            fightBoard2.EndCinematic();
        }
    }
}