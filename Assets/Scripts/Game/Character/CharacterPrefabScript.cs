using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterPrefabScript : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public BoardCharacter boardCharacter;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer spriteSocle;
    public Slider healthSlider;
    public Slider kiSlider;
    public Board assignedBoard;
    public Vector2Int position;
    private Material startMaterial;

    public Transform starContainer;
    public Sprite starImage;

    public void Start()
    {
        startMaterial = spriteRenderer.material;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (boardCharacter != null && boardCharacter.character.IsDead() == false)
        {
            var newMaterial = new Material(ShadersDatabase.Instance.outlineMaterial);
            spriteRenderer.material = newMaterial;
            spriteRenderer.material.SetColor("_OutlineColor", boardCharacter.isPlayerCharacter ? Color.green : Color.red);
            spriteRenderer.material.SetTexture("_MainTex", spriteRenderer.sprite.texture);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (boardCharacter != null && boardCharacter.character.IsDead() == false)
        {
            spriteRenderer.material = startMaterial;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        BoardGameUiManager.Instance.characterBoardUi.ShowCharacterBoard(boardCharacter.character);
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            LeanTween.cancel(CharacterDragInfo.draggedObject);
            if (hit.collider != null)
            {
                TileBehaviour tileScript = hit.collider.GetComponent<TileBehaviour>();
                if (tileScript != null && tileScript.position.x <= 4)
                {
                    LeanTween.move(CharacterDragInfo.draggedObject, tileScript.gameObject.transform.position, 0.1f).setEaseOutSine();
                    return;
                }
            }
            LeanTween.move(CharacterDragInfo.draggedObject, Camera.main.ScreenToWorldPoint(mousePosition), 0.1f);
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

                if (tileScript == null || tileScript.position.x > 4)
                {
                    return;
                }
                
                if (tileScript != null)
                {
                    var characterFrom =
                        GameManager.Instance.boardCharacterArray[tileScript.position.x, tileScript.position.y];
                    GameManager.Instance.boardCharacterArray[tileScript.position.x, tileScript.position.y] = boardCharacter;
                    GameManager.Instance.boardCharacterArray[position.x, position.y] = characterFrom;
                    boardCharacter.board.CreateBoard(GameManager.Instance.boardCharacterArray);
                }
            }
        }
    }
}

public static class CharacterDragInfo
{
    public static GameObject draggedObject;
    public static Vector2Int canPlayOnBoardPosition = new Vector2Int(-1, -1);
}