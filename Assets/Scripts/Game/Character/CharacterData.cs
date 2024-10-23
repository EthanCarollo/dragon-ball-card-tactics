
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Character/CharacterData")]
public class CharacterData : ScriptableObject
{
    public string characterName;

    public Sprite characterSprite;
    public GameObject characterPrefab;
    
    public int maxHealth;
    
    public int baseDamage;
    public int baseArmor;
    public int baseSpeed;
    public int baseAttackSpeed;

    public AnimationClip idle;
    public AnimationClip run;
    public AnimationClip attack;
}
