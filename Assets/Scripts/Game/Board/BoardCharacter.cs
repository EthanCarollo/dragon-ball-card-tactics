using System;
using UnityEngine;

[Serializable]
public class BoardCharacter : BoardObject
{
    public CharacterData character;

    public int actualHealth;
    public BoardCharacterState state;
    public bool isPlayerCharacter;
    public Vector2 direction;
    // If the nextPosition is to negative infinity, it just don't have a next position at all
    public Vector2Int nextPosition = new Vector2Int(-1, -1);

    public BoardCharacter(CharacterData character, bool isPlayerCharacter)
    {
        this.character = character;
        actualHealth = this.character.maxHealth;
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

    public void HitDamage(int damageAmount)
    {
        actualHealth -= damageAmount;
        SetCharacterSlider();
    }

    public void SetCharacterSlider()
    {
        gameObject.transform.GetChild(0).GetComponent<CharacterPrefabScript>().healthSlider.value = actualHealth;
    }
}
