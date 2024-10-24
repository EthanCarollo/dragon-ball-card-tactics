using System;
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
    public int GetAttackSpeed()
    {
        return character.baseAttackSpeed;
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

    public void Attack()
    {
        state.Attack();
    }

    public void SpecialAttack()
    {
        state.SpecialAttack();
    }

    public void HitDamage(int damageAmount)
    {
        actualHealth -= damageAmount;
        SetCharacterSlider();
    }

    public void Disappear()
    {
        
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
}
