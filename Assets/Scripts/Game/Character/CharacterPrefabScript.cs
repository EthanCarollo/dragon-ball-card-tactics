using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterPrefabScript : MonoBehaviour, IPointerClickHandler
{
    public BoardCharacter boardCharacter;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer spriteSocle;
    public Slider healthSlider;

    public void HitDamage(int damageMultiplicator)
    {
        boardCharacter.Attack();
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        BoardGameUiManager.Instance.characterBoardUi.ShowCharacterBoard(boardCharacter);
    }
}