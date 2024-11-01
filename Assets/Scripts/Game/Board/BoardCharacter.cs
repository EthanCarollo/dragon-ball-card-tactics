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

    public void HitDamage(int damageAmount)
    {
        
        this.character.actualHealth -= damageAmount;
        SetCharacterSlider();
        if (this.character.IsDead() && isDying == false)
        {
            Disappear();    
        }
    }

    public void Disappear()
    {
        isDying = true;
        var spriteRenderer = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRenderer.material = new Material(ShadersDatabase.Instance.disappearMaterial);
        spriteRenderer.material.SetFloat("_Fade", 1f);
        LeanTween.value(gameObject, f =>
        {
            spriteRenderer.material.SetFloat("_Fade", f);
        }, 1f, 0f, 2f)
        .setOnComplete((o =>
        {
            GameObject.Destroy(gameObject);
        }));
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

    public void PlayAnimationIfNotRunning(BoardAnimation animation)
    {
        if (!isAnimating)
        {
            gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().StartCoroutine(animation.PlayAnimationCoroutine(this));
        }
    }
}
