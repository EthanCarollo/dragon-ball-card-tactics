
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Character/CharacterData")]
public class CharacterData : ScriptableObject
{
    public int starNumber = 1;
    public string characterName;

    public Sprite characterSprite;
    public GameObject characterPrefab;
    
    public int maxHealth;
    public int maxKi = 50;
    
    public int baseDamage;
    public int baseArmor;
    public int baseSpeed;
    public float baseAttackSpeed;
    // baseCriticalChance is a percentage
    public int baseCriticalChance;

    public Sprite[] idleSprites;
    public Sprite[] runSprites;
    public Sprite[] attackSprites;
    public Sprite[] criticalAttackSprites;
    public Sprite[] superAttackSprites;
}
