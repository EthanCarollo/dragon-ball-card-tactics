using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CharacterPrefabScript : MonoBehaviour, IPointerClickHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public BoardCharacter boardCharacter;
    public SpriteRenderer spriteRenderer;
    public SpriteRenderer spriteSocle;
    public Slider healthSlider;
    public Slider kiSlider;
    public Board assignedBoard;
    public Vector2Int position;
    public Material startMaterial;
    public TextMeshProUGUI effectText;

    public Transform starContainer;
    public Sprite starImage;

    public void Start()
    {
        effectText.text = "";
        startMaterial = spriteRenderer.material;
        boardCharacter.PlayAnimation(SpriteDatabase.Instance.appearAnimation);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsValidBoardCharacter())
        {
            ApplyOutlineMaterial(boardCharacter.character.isPlayerCharacter ? Color.green : Color.red);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsValidBoardCharacter())
        {
            ResetMaterial();
        }
    }

    public bool IsValidBoardCharacter()
    {
        return boardCharacter != null && boardCharacter.character.IsDead() == false;
    }

    public void ApplyOutlineMaterial(Color outlineColor)
    {
        var newMaterial = new Material(ShadersDatabase.Instance.outlineMaterial);
        spriteRenderer.material = newMaterial;
        spriteRenderer.material.SetColor("_OutlineColor", outlineColor);
        spriteRenderer.material.SetTexture("_MainTex", spriteRenderer.sprite.texture);
    }

    public void ResetMaterial()
    {
        spriteRenderer.material = startMaterial;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        BoardGameUiManager.Instance.characterBoardUi.ShowCharacterBoard(boardCharacter.character);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (FightBoard.Instance.IsFighting() || boardCharacter.character.isPlayerCharacter == false)
        {
            return;
        }
        if (CharacterDragInfo.draggedObject == null)
        {
            Debug.Log("Drag a character");
            CharacterDragInfo.canPlayOnBoardPosition = new Vector2Int(-1, -1);
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
                CharacterPrefabScript characterPrefab = hit.collider.GetComponent<CharacterPrefabScript>();
                if (characterPrefab != null && characterPrefab.position.x <= 4){
                    LeanTween.move(CharacterDragInfo.draggedObject, characterPrefab.position, 0.1f).setEaseOutSine();
                    return;
                }
            }
            LeanTween.move(CharacterDragInfo.draggedObject, Camera.main.ScreenToWorldPoint(mousePosition), 0.1f);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (FightBoard.Instance.IsFighting() || boardCharacter.character.isPlayerCharacter == false)
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
                
                if (tileScript != null && tileScript.position.x <= 4)
                {
                    var characterFrom =
                        GameManager.Instance.boardCharacterArray[tileScript.position.x, tileScript.position.y];
                    GameManager.Instance.boardCharacterArray[tileScript.position.x, tileScript.position.y] = boardCharacter;
                    GameManager.Instance.boardCharacterArray[position.x, position.y] = characterFrom;
                    boardCharacter.board.CreateBoard(GameManager.Instance.boardCharacterArray);
                }

                CharacterPrefabScript characterPrefab = hit.collider.GetComponent<CharacterPrefabScript>();
                
                if (characterPrefab != null && characterPrefab.position.x <= 4)
                {
                    var characterFrom =
                        GameManager.Instance.boardCharacterArray[characterPrefab.position.x, characterPrefab.position.y];
                    GameManager.Instance.boardCharacterArray[characterPrefab.position.x, characterPrefab.position.y] = boardCharacter;
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