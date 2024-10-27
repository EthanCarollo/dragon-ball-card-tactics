using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class BoardCharacter : BoardObject
{
    public CharacterData character;

    public int actualHealth;
    public int actualKi;
    public BoardCharacterState state;
    public bool isPlayerCharacter;
    public Vector2 direction;
    // If the nextPosition is to negative infinity, it just don't have a next position at all
    public Vector2Int nextPosition = new Vector2Int(-1, -1);
    public bool isAnimating = false;

    public bool isDying = false;
    public bool IsDead()
    {
        return actualHealth <= 0;
    }
    public int GetAttackDamage()
    {
        return character.baseDamage;
    }
    public int GetArmor()
    {
        return character.baseArmor;
    }
    public int GetSpeed()
    {
        return character.baseSpeed;
    }
    public float GetAttackSpeed()
    {
        return character.baseAttackSpeed;
    }
    public int GetCriticalChance()
    {
        return character.baseCriticalChance;
    }
    public int GetRange()
    {
        return character.baseRange;
    }
    public BoardCharacter GetCharacterTarget()
    {
        if(state is AttackingCharacterState fightState){
            return fightState.characterTarget;
        }
        return null;
    }

    public BoardCharacter(CharacterData character, bool isPlayerCharacter)
    {
        this.character = character;
        actualHealth = this.character.maxHealth;
        actualKi = 0;
        state = new DefaultCharacterState(this);
        this.isPlayerCharacter = isPlayerCharacter;
        if (!isPlayerCharacter)
        {
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.left;
        }
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
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = direction == Vector2.right;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        state.Update();
    }

    public void Attack(Particle particle = null)
    {
        state.Attack(GetAttackDamage(), particle);
    }

    public void CriticalAttack(Particle particle = null)
    {
        state.Attack(GetAttackDamage() * 2, particle);
    }

    public void SpecialAttack(Particle particle = null)
    {
        state.Attack(GetAttackDamage(), particle);
    }

    public void HitDamage(int damageAmount)
    {
        
        actualHealth -= damageAmount;
        SetCharacterSlider();
        if (IsDead() && isDying == false)
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
        actualKi += kiAmount;
        if (actualKi > character.maxKi)
        {
            actualKi = character.maxKi;
        }
    }

    public void SetCharacterSlider()
    {
        var charPrefabScript = gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>();
        
        charPrefabScript.kiSlider.maxValue = character.maxKi;
        charPrefabScript.kiSlider.value = actualKi;
        charPrefabScript.healthSlider.maxValue = character.maxHealth;
        charPrefabScript.healthSlider.value = actualHealth;
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
