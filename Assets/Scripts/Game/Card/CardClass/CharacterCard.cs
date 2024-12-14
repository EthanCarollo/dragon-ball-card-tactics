using System;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterCard", menuName = "Card/CharacterCard")]
public class CharacterCard : Card
{
    public CharacterData character;

    public override string GetDescription()
    {
        return "Summons " + character.name + " on board.";
    }

    public override void UseCard(){
        if(CanUseCard() == false) {
            return;
        }
        MonoBehaviour.DestroyImmediate(CharacterDragInfo.draggedObject);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        
        if (hit.collider != null)
        {
            TileBehaviour tileScript = hit.collider.GetComponent<TileBehaviour>();

            if (tileScript.position.x > 4)
            {
                return;
            }
            
            CharacterPrefabScript characterScript = hit.collider.GetComponentInChildren<CharacterPrefabScript>();

            if (characterScript != null)
            {
                return;
            }

            if (tileScript != null)
            {
                tileScript.assignedBoard.AddCharacterFromBoard(new BoardCharacter(new CharacterContainer(character.id, new List<CharacterPassive>()), true), tileScript.position);
                GameManager.Instance.Player.Mana.CurrentMana -= manaCost;
                GameManager.Instance.RemoveCard(this);
                tileScript.assignedBoard.CreateBoard();
                BoardGameUiManager.Instance.RefreshUI();
                return;
            }
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (FightBoard.Instance.IsFighting())
        {
            return;
        }
        if (CharacterDragInfo.draggedObject == null)
        {
            Debug.Log("Drag a character");
            CharacterDragInfo.draggedObject = new GameObject("DraggedCharacter");
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f; 
            CharacterDragInfo.draggedObject.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
            SpriteRenderer spRenderer = CharacterDragInfo.draggedObject.AddComponent<SpriteRenderer>();
            spRenderer.sprite = character.characterSprite;
            spRenderer.color = new Color(1f, 1f, 1f, 0.5f);
            spRenderer.sortingOrder = 10;
        }
        else
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f; 
            CharacterDragInfo.draggedObject.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        
        if (FightBoard.Instance.IsFighting())
        {
            return;
        }
        if (CharacterDragInfo.draggedObject != null)
        {
            UseCard();
        }
    }
}