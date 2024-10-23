using UnityEngine;
using UnityEngine.UI;

public class CharacterPrefabScript : MonoBehaviour
{
    public BoardCharacter boardCharacter;
    public SpriteRenderer spriteRenderer;
    public Slider healthSlider;

    public void HitDamage(int damageMultiplicator)
    {
        boardCharacter.Attack();
    }
}