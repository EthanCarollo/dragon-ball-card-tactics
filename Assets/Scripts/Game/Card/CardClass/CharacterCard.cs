using System;
using UnityEngine.EventSystems;
using UnityEngine;

[Serializable]
public class CharacterCard : Card
{
    public CharacterContainer character;
    
    public override void UseCard(){
        
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
            spRenderer.sprite = character.GetCharacterData().characterSprite;
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
            MonoBehaviour.DestroyImmediate(CharacterDragInfo.draggedObject);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            
            if (hit.collider != null)
            {
                TileBehaviour tileScript = hit.collider.GetComponent<TileBehaviour>();

                if (tileScript.position.x > 4 && tileScript.assignedBoard is not VerticalBoard)
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
                    tileScript.assignedBoard.AddCharacterFromBoard(new BoardCharacter(character, true), tileScript.position);
                    GameManager.Instance.RemoveCard(this);
                    tileScript.assignedBoard.CreateBoard();
                    return;
                }
            }
        }
    }
}