using System;

[Serializable]
public class CharacterDeck
{
    public CharactersContainer characters = new CharactersContainer(12);
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

    public bool SwapCharacter(CharactersContainer charContainer, int indexFrom, int indexTarget)
    {
        var character1 = characters[indexFrom];
        var character2 = charContainer.characters[indexTarget];
        
        AddCharacter(character2, indexFrom);
        charContainer.AddCharacter(character1, indexTarget);

        return true;
    }
    
    public CharactersContainer Clone()
    {
        CharactersContainer clone = new CharactersContainer(characters.Length);
        clone.characters = (CharacterData[])this.characters.Clone();
        return clone;
    }
}
