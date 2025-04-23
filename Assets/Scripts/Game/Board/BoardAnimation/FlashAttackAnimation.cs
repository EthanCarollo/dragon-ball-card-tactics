using UnityEngine;
using System.Collections;
using System;



[CreateAssetMenu(fileName = "New Flash Attack Animation", menuName = "BoardAnimation/FlashAttackAnimation")]
public class FlashAttackAnimation : BoardAnimation
{
    public FrameSprite tpSprite;
    public AttackType attackType;
    public Particle particleAttack;
    public int kiOnAttack = 10;
    public Effect[] effectApplied;

    public override string GetDescription(CharacterContainer character)
    {
        return $"Teleports to all enemies and strikes them with a <color=#6A5ACD>{attackType}</color> attack, dealing <color=#D60000>{character.GetAttackDamage()}</color> damage and restoring <color=#007ACC>{kiOnAttack}</color> ki per hit.";
    }

    public override string GetDetailledDescription(CharacterContainer character)
    {
        return $"Teleports to all enemies and strikes them with a <color=#6A5ACD>{attackType}</color> attack, dealing <color=#D60000>{character.GetAttackDamage()}</color> damage and restoring <color=#007ACC>{kiOnAttack}</color> ki per hit.";
    }

    public override Sprite GetIcon(){
        if(animationIcon != null) return base.GetIcon();
        else return SpriteDatabase.Instance.flashAttackAbilityIcon;
    }

    public override IEnumerator PlayAnimationCoroutine(BoardCharacter character)
    {
        if (character.board is FightBoard fightBoard)
        {
            fightBoard.LaunchCinematic();
            CameraScript.Instance.SetupCameraOnTarget(4.5f, character.gameObject.transform);
        }
        
        character.actualAnimation = this;

        var targetToHit = GameManager.Instance.GetCharactersOnBoard().FindAll(boardChar => boardChar.character.isPlayerCharacter != character.character.isPlayerCharacter && boardChar.character.IsDead() == false);
        var actualPosition = character.gameObject.transform.position;

        foreach (var target in targetToHit)
        {
            if(target.gameObject == null) continue;
            yield return new WaitForSeconds(0.1f);
            try {
                var newPos = target.gameObject.transform.position;
                character.gameObject.transform.position = new Vector3(newPos.x - 1, newPos.y);
                character.gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().spriteRenderer.sprite = tpSprite.sprite;
            } catch(Exception error){
                Debug.LogWarning(error);
            }
            yield return new WaitForSeconds(0.1f);
            yield return AttackCharacter(character, target);
        }
        character.gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().spriteRenderer.sprite = tpSprite.sprite;
        yield return new WaitForSeconds(0.1f);
        character.gameObject.transform.position = actualPosition;
        EndAnimation(character);
    }

    private IEnumerator AttackCharacter(BoardCharacter character, BoardCharacter target){

        var index = 0;
        foreach (FrameSprite frameSprite in frameSprites)
        {
            character.gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().spriteRenderer.sprite = frameSprite.sprite;
            yield return new WaitForSeconds(frameSprite.time); 
            if(index == frameSprites.Length-1){
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
                                foreach(Effect effect in bonus.effectsOnHit){
                                    target.character.AddEffect(effect);
                                }
                            }
                        } else {
                            foreach(Effect effect in bonus.effectsOnHit){
                                target.character.AddEffect(effect);
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
    }

    public override void EndAnimation(BoardCharacter character)
    {
        base.EndAnimation(character);
        CameraScript.Instance.SetupFightCamera();
        
        if (character.board is FightBoard fightBoard2)
        {
            fightBoard2.EndCinematic();
        }
    }
}