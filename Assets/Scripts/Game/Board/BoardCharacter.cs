using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class BoardCharacter : BoardObject
{
    public CharacterContainer character;
    public BoardCharacterState state;
    public bool isPlayerCharacter;
    public Vector2 direction;
    // If the nextPosition is to negative infinity, it just don't have a next position at all
    public Vector2Int nextPosition = new Vector2Int(-1, -1);
    public bool isAnimating = false;

    public bool isDying = false;
    public BoardCharacter GetCharacterTarget()
    {
        if(state is AttackingCharacterState fightState){
            return fightState.characterTarget;
        }
        return null;
    }

    public BoardCharacter(CharacterContainer character, bool isPlayerCharacter)
    {
        SetupCharacter(character);
        this.isPlayerCharacter = isPlayerCharacter;
        if (!isPlayerCharacter)
        {
            direction = Vector2.left;
        }
        else
        {
            direction = Vector2.right;
        }
    }

    public void SetupCharacter(CharacterContainer character)
    {
        this.character = character;
        state = new DefaultCharacterState(this);
    }

    public void UpdateState(BoardCharacterState newState)
    {
        state = newState;
    }

    public override void UpdateUi()
    {
        
    }

    public override void Update()
    {
        try
        {
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = direction == Vector2.left;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        state.Update();
    }

    public void Attack(Particle particle = null)
    {
        state.Attack(this.character.GetAttackDamage(), particle);
    }

    public void CriticalAttack(Particle particle = null)
    {
        state.Attack(this.character.GetAttackDamage() * 2, particle);
    }

    public void SpecialAttack(Particle particle = null)
    {
        state.Attack(this.character.GetAttackDamage(), particle);
    }

    public void EndKikoha()
    {
        state.EndKikoha();
    }

    public void Dead()
    {
        state.Dead();
    }

    public void ResetGameObjectPosition()
    {
        
    }

    public void HitDamage(int damageAmount)
    {
        this.character.actualHealth -= damageAmount;
        ParticleManager.Instance.ShowAttackNumber(this, damageAmount);
        SetCharacterSlider();
        if (this.character.IsDead() && isDying == false)
        {
            this.Dead(); 
        }
    }

    public void AddKi(int kiAmount)
    {
        this.character.actualKi += kiAmount;
        if (this.character.actualKi > this.character.GetCharacterData().maxKi)
        {
            this.character.actualKi = this.character.GetCharacterData().maxKi;
        }
    }

    public void SetCharacterSlider()
    {
        var charPrefabScript = gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>();
        
        charPrefabScript.kiSlider.maxValue = this.character.GetCharacterData().maxKi;
        charPrefabScript.kiSlider.value = this.character.actualKi;
        charPrefabScript.healthSlider.maxValue = this.character.GetCharacterData().maxHealth;
        charPrefabScript.healthSlider.value = this.character.actualHealth;
    }

    public void PlayAnimation(BoardAnimation animation)
    {
        gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().StopAllCoroutines();
        gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().StartCoroutine(animation.PlayAnimationCoroutine(this));
    }


    public void PlayAnimation(BoardAnimation animation, Action onAnimationComplete = null)
    {
        var characterScript = gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>();
        characterScript.StopAllCoroutines();
        characterScript.StartCoroutine(PlayAnimationWithCallback(animation, onAnimationComplete));
    }

    private IEnumerator PlayAnimationWithCallback(BoardAnimation animation, Action onAnimationComplete)
    {
        yield return gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().StartCoroutine(animation.PlayAnimationCoroutine(this));
        onAnimationComplete?.Invoke();
    }

    public void PlayAnimationIfNotRunning(BoardAnimation animation)
    {
        if (!isAnimating)
        {
            gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().StartCoroutine(animation.PlayAnimationCoroutine(this));
        }
    }

    public void LaunchKikoha() 
    {
        state.LaunchKikoha();
    }

    public void UpdateKikohaAdvancement(int percentage){
        state.UpdateKikohaAdvancement(percentage);
    }

    public int GetKikohaAdvancement(){
        return state.GetKikohaAdvancement();
    }

    public override BoardObject Clone()
    {
        BoardCharacter clonedCharacter = new BoardCharacter(character, isPlayerCharacter);
        clonedCharacter.SetGameObject(this.gameObject);
        clonedCharacter.SetBoard(this.board); 
        return clonedCharacter;
    }
}
