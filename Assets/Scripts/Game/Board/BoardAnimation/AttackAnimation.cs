using UnityEngine;
using System.Collections;
using System;



[CreateAssetMenu(fileName = "New Attack Animation", menuName = "BoardAnimation/AttackAnimation")]
public class AttackAnimation : BoardAnimation
{
    public int attackFrameIndex;
    public AttackType attackType;
    public Particle particleAttack;
    public int kiOnAttack = 10;
    public bool isCinematic = false;

    public Effect[] effectApplied;

    public override string GetDescription(CharacterContainer character)
    {
        int damage = character.GetAttackDamage();
        return $"Performs an attack with a <color=#6A5ACD>{attackType}</color> strike, dealing <color=#D60000>{damage}</color> damage and restoring <color=#007ACC>{kiOnAttack}</color> ki.";
    }


    public override IEnumerator PlayAnimationCoroutine(BoardCharacter character)
    {
        var target = character.GetCharacterTarget();
        if (character.board is FightBoard fightBoard && isCinematic)
        {
            fightBoard.LaunchCinematic();
            CameraScript.Instance.SetupCameraOnTarget(4.5f, character.gameObject.transform);
        }
        
        character.actualAnimation = this;
        var index = 0;
        foreach (FrameSprite frameSprite in frameSprites)
        {
            character.gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().spriteRenderer.sprite = frameSprite.sprite;
            yield return new WaitForSeconds(frameSprite.time); 
            if(index == attackFrameIndex){
                if(effectApplied != null && effectApplied.Length > 0)
                {
                    foreach (var effect in effectApplied)
                    {
                        target.character.AddEffect(effect);
                    }
                }
                
                try {
                    var activeBonuses = character.character.GetAllActiveBonuses();
                    foreach (var bonus in activeBonuses)
                    {
                        if(bonus is SpecialCharacterBonus specialCharacterBonus){ 
                            if(specialCharacterBonus.character == character.character.GetCharacterData()){
                                if(bonus.effectsOnHit != null){
                                    foreach(Effect effect in bonus.effectsOnHit){
                                        target.character.AddEffect(effect);
                                    }
                                }
                            }
                        } else {
                            if(bonus.effectsOnHit != null){
                                foreach(Effect effect in bonus.effectsOnHit){
                                    if(effect != null) target.character.AddEffect(effect);
                                }
                            }
                        }
                    }
                } catch(Exception error){
                    Debug.LogError("Cannot inflict effect , " + error);
                }

                character.AddKi(kiOnAttack);
                switch (attackType)
                {
                    case AttackType.Normal :
                        character.Attack(1, particleAttack, target);
                        break;
                    case AttackType.Critical :
                        character.Attack(2, particleAttack, target);
                        break;
                    case AttackType.Special :
                        character.Attack(4, particleAttack, target);
                        break;
                }
            }
            index++;
        }
        
        EndAnimation(character);
    }

    public override void EndAnimation(BoardCharacter character)
    {
        base.EndAnimation(character);
        
        if (character.board is FightBoard fightBoard2 && isCinematic)
        {
            fightBoard2.EndCinematic();
        }
    }
}