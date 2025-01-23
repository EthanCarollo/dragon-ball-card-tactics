using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Synergy", menuName = "Synergy/CharacterSynergy")]
public class Synergy : ScriptableObject {
    public string synergyName;
    public Sprite synergyImage;
    public List<TierBonus> Tiers = new List<TierBonus>();

    public List<TierBonus> GetActiveTierBonuses(bool isPlayerCharacter){
        List<TierBonus> tierBonuses = new List<TierBonus>();
        foreach(var tierBonus in Tiers){
            if(GetActiveUnit(isPlayerCharacter) >= tierBonus.RequiredUnits){
                tierBonuses.Add(tierBonus);
            }
        }
        return tierBonuses;
    }

    public string GetDescription(){
        string description = "<size=20>" + synergyName + "</size>";
        int iterator = 0;
        foreach(var tierBonus in Tiers){
            iterator++;
            description += "<size=14>\n\n" + "Tier " + iterator.ToString() + " - " + tierBonus.RequiredUnits + " Unit</size>";
            description += "<size=10>\n" + tierBonus.Description + "</size>";
        }
        return description;
    }

    public int GetActiveUnit(bool isPlayerCharacter = true){
        int activeUnit = 0;
        var boardCharacters = GameManager.Instance.GetCharactersOnBoard();
        foreach (var boardCharacter in boardCharacters)
        {
            var synergies = boardCharacter.character.GetSynergies();
            if(synergies == null) continue;
            if(boardCharacter.character.isPlayerCharacter == isPlayerCharacter && synergies.Contains(this)){
                activeUnit++;
            }
        }
        return activeUnit;
    }

    public (List<BoardCharacter> boardCharactersWithSynergy, List<CharacterData> databaseCharactersWithSynergy) GetCharactersWithSynergy()
    {
        // Characters on the board with the synergy
        var boardCharactersWithSynergy = GameManager.Instance.GetCharactersOnBoard()
            .Where(boardCharacter =>
                boardCharacter.character.isPlayerCharacter &&
                boardCharacter.character.GetSynergies() != null &&
                boardCharacter.character.GetSynergies().Contains(this))
            .ToList();

        // Characters in the CharacterDatabase with the synergy
        var databaseCharactersWithSynergy = CharacterDatabase.Instance.characterDatas
            .Where(characterData =>
                characterData.synergies != null &&
                characterData.synergies.Contains(this))
            .ToList();

        return (boardCharactersWithSynergy, databaseCharactersWithSynergy);
    }
}

[Serializable]
public class TierBonus
{
    public int RequiredUnits;  
    [TextArea]
    public string Description;

    [SerializeReference, SubclassSelector]
    public List<Bonus> Bonuses;
}