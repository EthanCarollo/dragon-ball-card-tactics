using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "CharacterCard", menuName = "Card/CharacterCard")]
public class CharacterCard : Card
{
    public CharacterData character;

    public override string GetDescription()
    {
        return character == null ? "Summons an unconfigured character." : "Summons " + character.name + " on board.";
    }

    public override bool CanUseCard()
    {
        if (character == null || GameManager.Instance == null ||
            manaCost > GameManager.Instance.Player.Mana.CurrentMana)
        {
            return false;
        }

        var characterOnBoard = FindMatchingPlayerCharacter();
        if (characterOnBoard != null)
        {
            return characterOnBoard.character.CanAddStar();
        }

        return GameManager.Instance.GetCharactersOnBoard()
            .Count(boardCharacter => boardCharacter?.character != null && boardCharacter.character.isPlayerCharacter) <
            GameManager.Instance.Player.Level.maxUnit;
    }

    public override void UseCard()
    {
        BoardGameUiManager.Instance?.HidePlayCardPanel();
        if (!CanUseCard() || Camera.main == null || FightBoard.Instance == null)
        {
            return;
        }

        if (CharacterDragInfo.draggedObject != null)
        {
            MonoBehaviour.Destroy(CharacterDragInfo.draggedObject);
            CharacterDragInfo.draggedObject = null;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            TileBehaviour tileScript = hit.collider.GetComponent<TileBehaviour>();
            CharacterPrefabScript characterScript = hit.collider.GetComponentInChildren<CharacterPrefabScript>();
            if (tileScript == null && characterScript == null)
            {
                return;
            }

            BoardCharacter characterExist = FindMatchingPlayerCharacter();
            if (characterExist != null)
            {
                if (!characterExist.character.CanAddStar())
                {
                    return;
                }

                characterExist.character.AddStar();
                characterExist.SetCharacterSlider();
                GameManager.Instance.Player.Mana.CurrentMana -= manaCost;
                BoardGameUiManager.Instance?.ShowLooseMana(manaCost);
                RegisterCardHistory();
                GameManager.Instance.RemoveCard(this);
                BoardGameUiManager.Instance?.RefreshUI();
                FightBoard.Instance.CreateBoard(GameManager.Instance.boardCharacterArray);
                return;
            }

            if (tileScript == null || tileScript.position.x > 4 || tileScript.assignedBoard == null)
            {
                return;
            }

            var board = GameManager.Instance.boardCharacterArray;
            if (board == null || tileScript.position.x < 0 || tileScript.position.x >= board.GetLength(0) ||
                tileScript.position.y < 0 || tileScript.position.y >= board.GetLength(1) ||
                board[tileScript.position.x, tileScript.position.y] != null)
            {
                return;
            }

            var boardCharacter = new BoardCharacter(
                new CharacterContainer(character.id, new List<CharacterPassive>(), 1, true));
            if (!tileScript.assignedBoard.AddCharacterFromBoard(boardCharacter, tileScript.position))
            {
                return;
            }

            GameManager.Instance.Player.Mana.CurrentMana -= manaCost;
            BoardGameUiManager.Instance?.ShowLooseMana(manaCost);
            RegisterCardHistory();
            GameManager.Instance.RemoveCard(this);
            tileScript.assignedBoard.CreateBoard(GameManager.Instance.boardCharacterArray);
            BoardGameUiManager.Instance?.RefreshUI();
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        CharacterDragInfo.canPlayOnBoardPosition = new Vector2Int(-1, -1);
        BoardCharacter characterExist = FindMatchingPlayerCharacter();

        if(characterExist != null){
            var positionCharacter = BoardUtils.FindPosition(GameManager.Instance.boardCharacterArray, characterExist);
            CharacterDragInfo.canPlayOnBoardPosition = positionCharacter;
        }
        if(CanUseCard() == false) {
            return;
        }
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (!CanUseCard() || FightBoard.Instance == null || FightBoard.Instance.IsFighting() ||
            Camera.main == null || GameManager.Instance == null)
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            LeanTween.cancel(CharacterDragInfo.draggedObject);

            if (hit.collider != null)
            {
                TileBehaviour tileScript = hit.collider.GetComponent<TileBehaviour>();
                if(hit.collider.GetComponent<TileBehaviour>() || hit.collider.GetComponent<CharacterPrefabScript>()){
                    BoardCharacter characterExist = FindMatchingPlayerCharacter();
                    
                    if(characterExist != null){
                        var positionCharacter = BoardUtils.FindPosition(GameManager.Instance.boardCharacterArray, characterExist);
                        CharacterDragInfo.canPlayOnBoardPosition = positionCharacter;
                        var shaderDatabase = ShadersDatabase.Instance;
                        var draggedRenderer = CharacterDragInfo.draggedObject.GetComponent<SpriteRenderer>();
                        if (shaderDatabase != null && shaderDatabase.outlineMaterial != null && draggedRenderer != null)
                        {
                            draggedRenderer.material = new Material(shaderDatabase.outlineMaterial);
                            draggedRenderer.material.SetColor("_OutlineColor", Color.white);
                            if (draggedRenderer.sprite != null)
                            {
                                draggedRenderer.material.SetTexture("_MainTex", draggedRenderer.sprite.texture);
                            }
                        }
                        LeanTween.move(CharacterDragInfo.draggedObject, new Vector3(positionCharacter.x, positionCharacter.y, 0f), 0.1f).setEaseOutSine();
                        BoardGameUiManager.Instance?.ShowPlayCardPanel();
                        return;
                    }
                }
                if (tileScript != null && tileScript.position.x <= 4 && 
                    GameManager.Instance.boardCharacterArray[tileScript.position.x, tileScript.position.y] == null)
                {
                    LeanTween.move(CharacterDragInfo.draggedObject, tileScript.gameObject.transform.position, 0.1f).setEaseOutSine();
                    return;
                }
            }
            
            BoardGameUiManager.Instance?.HidePlayCardPanel();
            var fallbackShaderDatabase = ShadersDatabase.Instance;
            var fallbackDraggedRenderer = CharacterDragInfo.draggedObject.GetComponent<SpriteRenderer>();
            if (fallbackShaderDatabase != null && fallbackShaderDatabase.spriteMaterial != null && fallbackDraggedRenderer != null)
            {
                fallbackDraggedRenderer.material = new Material(fallbackShaderDatabase.spriteMaterial);
            }
            LeanTween.move(CharacterDragInfo.draggedObject, Camera.main.ScreenToWorldPoint(mousePosition), 0.1f);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!CanUseCard())
        {
            return;
        }
        if (FightBoard.Instance == null || FightBoard.Instance.IsFighting())
        {
            return;
        }
        if (CharacterDragInfo.draggedObject != null)
        {
            UseCard();
        }
    }

    public override string GetCardType()
    {
        return "Character";
    }

    private BoardCharacter FindMatchingPlayerCharacter()
    {
        if (character == null || GameManager.Instance == null)
        {
            return null;
        }

        return GameManager.Instance.GetCharactersOnBoard()
            .Where(boardCharacter => boardCharacter?.character != null && boardCharacter.character.isPlayerCharacter)
            .FirstOrDefault(boardCharacter => IsSameCharacterFamily(
                boardCharacter.character.GetCharacterData(), character));
    }

    private static bool IsSameCharacterFamily(CharacterData first, CharacterData second)
    {
        if (first == null || second == null)
        {
            return false;
        }

        return first == second ||
               (first.sameCharacters != null && first.sameCharacters.Contains(second)) ||
               (second.sameCharacters != null && second.sameCharacters.Contains(first));
    }
}
