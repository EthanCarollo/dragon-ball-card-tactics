using System;

[Serializable]
public class CharacterDeck
{
    public CharacterData[] oneStarCharacters = new CharacterData[4];
    public CharacterData[] twoStarCharacters = new CharacterData[3];
    public CharacterData[] threeStarCharacters = new CharacterData[2];
    public CharacterData[] fourStarCharacters = new CharacterData[1];
}
