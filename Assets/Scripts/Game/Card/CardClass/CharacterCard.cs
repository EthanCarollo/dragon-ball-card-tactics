using System;
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
        return "Summons " + character.name + " on board.";
    }

    public override bool CanUseCard(){
        if(manaCost > GameManager.Instance.Player.Mana.CurrentMana){
            return false;
        }
        BoardCharacter characterExist = GameManager.Instance.GetCharactersOnBoard()
            .Where(cha => cha.isPlayerCharacter).ToList().Find(cha => cha.character.GetCharacterData() == character);
        if(characterExist != null){
            return true;
        }
        if(GameManager.Instance.Player.Level.maxUnit <= GameManager.Instance.GetCharactersOnBoard().Where(character => character.isPlayerCharacter).Count()){
            return false;
        }
        return true;
    }

    public override void UseCard(){
        BoardGameUiManager.Instance.HidePlayCardPanel();
        if(CanUseCard() == false) {
            return;
        }
        MonoBehaviour.DestroyImmediate(CharacterDragInfo.draggedObject);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            TileBehaviour tileScript = hit.collider.GetComponent<TileBehaviour>();
            if (tileScript == null) return;


            BoardCharacter characterExist = GameManager.Instance.GetCharactersOnBoard()
                    .Where(cha => cha.isPlayerCharacter).ToList().Find(cha => cha.character.GetCharacterData() == character);
            if(characterExist != null){
                characterExist.character.AddStar(1);
                GameManager.Instance.Player.Mana.CurrentMana -= manaCost;
                BoardGameUiManager.Instance.ShowLooseMana(manaCost);
                GameManager.Instance.RemoveCard(this);
                BoardGameUiManager.Instance.RefreshUI();
                FightBoard.Instance.CreateBoard(GameManager.Instance.boardCharacterArray);
                return;
            }

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
                tileScript.assignedBoard.AddCharacterFromBoard(new BoardCharacter(new CharacterContainer(character.id, new List<CharacterPassive>(), 1), true), tileScript.position);
                GameManager.Instance.Player.Mana.CurrentMana -= manaCost;
                BoardGameUiManager.Instance.ShowLooseMana(manaCost);
                GameManager.Instance.RemoveCard(this);
                tileScript.assignedBoard.CreateBoard(GameManager.Instance.boardCharacterArray);
                BoardGameUiManager.Instance.RefreshUI();
                return;
            }
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        CharacterDragInfo.canPlayOnBoardPosition = new Vector2Int(-1, -1);
        BoardCharacter characterExist = GameManager.Instance.GetCharactersOnBoard()
            .Where(cha => cha.isPlayerCharacter).ToList().Find(cha => cha.character.GetCharacterData() == character);
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
        if(CanUseCard() == false) {
            return;
        }
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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            LeanTween.cancel(CharacterDragInfo.draggedObject);

            if (hit.collider != null)
            {
                TileBehaviour tileScript = hit.collider.GetComponent<TileBehaviour>();
                if(tileScript != null){
                    BoardCharacter characterExist = GameManager.Instance.GetCharactersOnBoard()
                        .Where(cha => cha.isPlayerCharacter).ToList().Find(cha => cha.character.GetCharacterData() == character);
                    if(characterExist != null){
                        var positionCharacter = BoardUtils.FindPosition(GameManager.Instance.boardCharacterArray, characterExist);
                        CharacterDragInfo.canPlayOnBoardPosition = positionCharacter;
                        CharacterDragInfo.draggedObject.GetComponent<SpriteRenderer>().material = new Material(ShadersDatabase.Instance.outlineMaterial);
                        CharacterDragInfo.draggedObject.GetComponent<SpriteRenderer>().material.SetColor("_OutlineColor", Color.white);
                        CharacterDragInfo.draggedObject.GetComponent<SpriteRenderer>().material.SetTexture("_MainTex", CharacterDragInfo.draggedObject.GetComponent<SpriteRenderer>().sprite.texture);
                        LeanTween.move(CharacterDragInfo.draggedObject, new Vector3(positionCharacter.x, positionCharacter.y, 0f), 0.1f).setEaseOutSine();
                        BoardGameUiManager.Instance.ShowPlayCardPanel();
                        return;
                    }
                }
                if (tileScript != null && tileScript.position.x <= 4)
                {
                    LeanTween.move(CharacterDragInfo.draggedObject, tileScript.gameObject.transform.position, 0.1f).setEaseOutSine();
                    return;
                }
            }
            
            BoardGameUiManager.Instance.HidePlayCardPanel();
            CharacterDragInfo.draggedObject.GetComponent<SpriteRenderer>().material = new Material(ShadersDatabase.Instance.spriteMaterial);
            LeanTween.move(CharacterDragInfo.draggedObject, Camera.main.ScreenToWorldPoint(mousePosition), 0.1f);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if(CanUseCard() == false) {
            return;
        }
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