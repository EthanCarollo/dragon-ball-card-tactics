
using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Character/CharacterData")]
public class CharacterData : ScriptableObject
{
    // The starNumber is also the cost of the "card"
    public int id = 0;
    public int starNumber = 1;
    public string characterName;
    public CharacterRole role = CharacterRole.DPS;
    public CharacterType characterType = CharacterType.Strength;

    public Sprite characterSprite;
    public Sprite characterIcon;
    public GameObject characterPrefab;
    
    public int maxHealth = 250;
    public int maxKi = 50;
    
    public int baseRange = 1;
    public int baseDamage = 50;
    public int baseArmor = 25;
    public int baseSpeed = 2;
    public float baseAttackSpeed = 0.5f;
    public int baseCriticalChance = 10;

    public BoardAnimation idleAnimation;
    public BoardAnimation runAnimation;
    public BoardAnimation attackAnimation;
    public BoardAnimation criticalAttackAnimation;
    public SpecialAttack[] specialAttackAnimation;
    public BoardAnimation winPoseAnimation;
    public BoardAnimation deadAnimation;
    
    // The base character, one without any transformation
    public CharacterData baseCharacter;
    public string spriteCredit;

    [FormerlySerializedAs("sameCharacter")] public CharacterData[] sameCharacters; // This said if it's the same char than another, useful if we want to upgrade one

    public Synergy[] synergies;
    
    public Color GetCharacterColor()
    {
        switch (characterType)
        {
            case CharacterType.Strength:
                return Color.red;
            case CharacterType.Technic:
                return Color.green;
            case CharacterType.Intelligence:
                return Color.magenta;
            case CharacterType.Endurance:
                return Color.yellow;
        }
        return Color.white;
    }

    public CharacterPassive defaultPassive;
}

[Serializable]
public class SpecialAttack
{
    public string name;
    public BoardAnimation animation;
}

public enum CharacterType
{
    Strength,
    Technic,
    Intelligence,
    Endurance
}

public enum CharacterRole
{
    DPS,
    TANK,
    SUPPORT
}
