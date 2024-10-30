using System;

[Serializable]
public class CharacterDeck
{
    public CharactersContainer oneStarCharacters = new CharactersContainer(4);
    public CharactersContainer twoStarCharacters = new CharactersContainer(3);
    public CharactersContainer threeStarCharacters = new CharactersContainer(2);
    public CharactersContainer fourStarCharacters = new CharactersContainer(1);
}

[Serializable]
public class CharactersContainer
{
    public CharacterData[] characters;

    public CharactersContainer(int size)
    {
        characters = new CharacterData[size];
    }
    
    public void AddCharacter(CharacterData character, int index)
    {
        this.characters[index] = character;
    }

    public void SwapCharacter(CharactersContainer charContainer, int indexFrom, int indexTarget)
    {
        var character1 = characters[indexFrom];
        var character2 = charContainer.characters[indexTarget];
        
        AddCharacter(character2, indexFrom);
        charContainer.AddCharacter(character1, indexTarget);
    }
}
