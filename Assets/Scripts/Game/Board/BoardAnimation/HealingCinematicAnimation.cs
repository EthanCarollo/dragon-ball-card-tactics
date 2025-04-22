using UnityEngine;
using System.Collections;
using System;



[CreateAssetMenu(fileName = "New Healing Animation", menuName = "BoardAnimation/HealingCinematicAnimation")]
public class HealingCinematicAnimation : BoardAnimation
{
    public int attackFrameIndex;
    public AttackType attackType;
    public Particle particleAttack;

    public override string GetDescription(CharacterContainer character)
    {
        int healingAmount = character.GetAttackDamage(); // Assuming healing uses attack damage as the base.
        return $"Performs a healing, restoring <color=#32CD32>{healingAmount}</color> health to the target using a <color=#6A5ACD>{attackType}</color> healing attack.";
    }

    public override IEnumerator PlayAnimationCoroutine(BoardCharacter character)
    {
        CameraScript.Instance.SetupCameraOnTarget(3.5f, character.gameObject.transform);
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
            if(index == attackFrameIndex){
                character.Heal(character.character.GetAttackDamage());
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