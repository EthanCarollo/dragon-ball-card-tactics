using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterPrefabScript : MonoBehaviour, IPointerClickHandler
{
    public BoardCharacter boardCharacter;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer spriteSocle;
    public Slider healthSlider;
    public Slider kiSlider;

    public void HitDamage(int damageMultiplicator)
    {
        boardCharacter.Attack();
    }

    public void HitSpecialDamage(int damageMultiplicator)
    {
        boardCharacter.SpecialAttack();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        BoardGameUiManager.Instance.characterBoardUi.ShowCharacterBoard(boardCharacter);
    }
}