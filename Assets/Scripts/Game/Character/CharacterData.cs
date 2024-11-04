
using System;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Character/CharacterData")]
public class CharacterData : ScriptableObject
{
    // The starNumber is also the cost of the "card"
    public int id = 0;
    public int starNumber = 1;
    public string characterName;
    public CharacterRole role = CharacterRole.DPS;

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
    // baseCriticalChance is a percentage
    public int baseCriticalChance = 10;

    public BoardAnimation idleAnimation;
    public BoardAnimation runAnimation;
    public BoardAnimation attackAnimation;
    public BoardAnimation criticalAttackAnimation;
    public SpecialAttack[] specialAttackAnimation;
}

[Serializable]
public class SpecialAttack
{
    public string name;
    public string description;
    public BoardAnimation animation;
}

public enum CharacterRole
{
    DPS,
    TANK
}
