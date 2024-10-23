using System;

[Serializable]
public class BoardCharacter
{
    public CharacterData character;

    public int actualHealth;

    public BoardCharacter(CharacterData character)
    {
        this.character = character;
        actualHealth = this.character.maxHealth;
    }
}
