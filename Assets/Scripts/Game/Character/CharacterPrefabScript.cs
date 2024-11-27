using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterPrefabScript : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler
{
    public BoardCharacter boardCharacter;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer spriteSocle;
    public Slider healthSlider;
    public Slider kiSlider;
    public Board assignedBoard;
    public Vector2Int position;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        BoardGameUiManager.Instance.characterBoardUi.ShowCharacterBoard(boardCharacter);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (FightBoard.Instance.IsFighting() || boardCharacter.isPlayerCharacter == false)
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
            spRenderer.sprite = boardCharacter.character.GetCharacterData().characterSprite;
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

    public void OnEndDrag(PointerEventData eventData)
    {
        if (FightBoard.Instance.IsFighting() || boardCharacter.isPlayerCharacter == false)
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
                
                if (tileScript != null)
                {
                    boardCharacter.board.RemoveCharacterFromBoard(boardCharacter);
                    tileScript.assignedBoard.AddCharacterFromBoard(boardCharacter, tileScript.position);
                    boardCharacter.board.CreateBoard();
                    tileScript.assignedBoard.CreateBoard();
                    return;
                }
                
                CharacterPrefabScript characterScript = hit.collider.GetComponentInChildren<CharacterPrefabScript>();

                if (characterScript != null)
                {
                    Debug.Log(characterScript);
                    this.assignedBoard.RemoveCharacterFromBoard(boardCharacter);
                    characterScript.assignedBoard.RemoveCharacterFromBoard(characterScript.boardCharacter);
                    
                    characterScript.assignedBoard.AddCharacterFromBoard(boardCharacter, characterScript.position);
                    this.assignedBoard.AddCharacterFromBoard(characterScript.boardCharacter, this.position);
                    characterScript.assignedBoard.CreateBoard();
                    this.assignedBoard.CreateBoard();
                    return;
                }
            }
        }
    }
}

public static class CharacterDragInfo
{
    public static GameObject draggedObject;
}