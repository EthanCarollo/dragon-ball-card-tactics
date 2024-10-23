using System;
using UnityEngine;

[Serializable]
public class BoardCharacter
{
    public CharacterData character;

    public int actualHealth;
    public GameObject gameObject;
    public Board board;
    public BoardCharacterState state;
    public bool isPlayerCharacter;
    public Vector2 direction;
    public Vector2 nextPosition;

    public BoardCharacter(CharacterData character, bool isPlayerCharacter)
    {
        this.character = character;
        actualHealth = this.character.maxHealth;
        state = new DefaultCharacterState(this);
        isPlayerCharacter = isPlayerCharacter;
        if (!isPlayerCharacter)
        {
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.left;
        }
    }

    public BoardCharacter SetGameObject(GameObject gameObject)
    {
        this.gameObject = gameObject;
        return this;
    }

    public BoardCharacter SetBoard(Board board)
    {
        this.board = board;
        return this;
    }


    public void Update()
    {
        try
        {
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = direction == Vector2.right;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
