using UnityEngine;
using System.Collections;
using System;



[CreateAssetMenu(fileName = "New Healing Other Animation", menuName = "BoardAnimation/HealingOtherCinematicAnimation")]
public class HealingOtherCinematicAnimation : BoardAnimation
{
    public int attackFrameIndex;
    public AttackType attackType;
    public Particle particleAttack;

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
                        // Ici c'est un heal de 20% des HP
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