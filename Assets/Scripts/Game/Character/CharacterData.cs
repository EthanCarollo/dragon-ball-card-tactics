
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

    [SerializeReference, SubclassSelector]
    public BoardAnimation idleAnimation;
    [SerializeReference, SubclassSelector]
    public BoardAnimation runAnimation;
    [SerializeReference, SubclassSelector]
    public BoardAnimation attackAnimation;
    [SerializeReference, SubclassSelector]
    public BoardAnimation criticalAttackAnimation;
    [SerializeReference, SubclassSelector]
    public BoardAnimation specialAttackAnimation;
}

public enum CharacterRole
{
    DPS,
    TANK
}
