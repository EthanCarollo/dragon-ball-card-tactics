using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Coffee.UIEffects;
using TMPro;

public abstract class UsableCharacterActionCard : Card
{
    public CharacterData characterFor;

    protected BoardCharacter GetCharacterOnMouse()
    {
        if (Camera.main == null || GameManager.Instance == null)
        {
            return null;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        
        if (hit.collider != null && 
            (hit.collider.GetComponent<TileBehaviour>() != null || hit.collider.GetComponent<CharacterPrefabScript>() != null))
        {
            var charactersUpdatable =  GameManager.Instance.GetCharactersOnBoard()
                .Where(cha => cha?.character != null && cha.character.isPlayerCharacter && cha.gameObject != null)
                .ToList()
                .FindAll(cha =>
                {
                    if (characterFor == null) return true;
                    return cha.character.GetCharacterData() == characterFor;
                });
            if(charactersUpdatable.Count == 0) return null;

            var mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return GetClosestPosition(charactersUpdatable, mousePosInWorld);
        }
        return null;
    }

    BoardCharacter GetClosestPosition(List<BoardCharacter> characters, Vector3 target)
    {
        BoardCharacter closest = characters[0];
        float closestDistance = Vector3.Distance(closest.gameObject.transform.position, target);

        foreach (var character in characters)
        {
            float distance = Vector3.Distance(character.gameObject.transform.position, target);
            if (distance < closestDistance)
            {
                closest = character;
                closestDistance = distance;
            }
        }

        return closest;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        // CameraScript.Instance.SetupFightCamera();
    }
    
    public override void OnDrag(PointerEventData eventData)
    {
        if (!CanUseCard() || Camera.main == null || GameManager.Instance == null)
        {
            return;
        }

        if (DraggedActionCard.DraggedCard == null)
        {
            Debug.Log("Drag a character");
            DraggedActionCard.DraggedCard = DraggedActionCard.InstantiateCard(this);
        }
        else
        {
            LeanTween.cancel(DraggedActionCard.DraggedCard);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            
            if (hit.collider != null && (hit.collider.GetComponent<TileBehaviour>() != null || hit.collider.GetComponent<CharacterPrefabScript>() != null))
            {
                BoardCharacter characterExist = GetCharacterOnMouse(); 
                if(characterExist != null){
                    var positionCharacter = BoardUtils.FindPosition(GameManager.Instance.boardCharacterArray, characterExist);
                    var characterUiPosition = Camera.main.WorldToScreenPoint(new Vector3(positionCharacter.x, positionCharacter.y + 2, 0f));
                    LeanTween.move(DraggedActionCard.DraggedCard, characterUiPosition, 0.1f).setEaseOutSine();
                    BoardGameUiManager.Instance?.ShowPlayCardPanel();
                    return;
                }
            }
            BoardGameUiManager.Instance?.HidePlayCardPanel();
            
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f; 
                LeanTween.move(DraggedActionCard.DraggedCard, new Vector3(mousePosition.x, mousePosition.y, 0f), 0.1f).setEaseOutSine();
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        BoardGameUiManager.Instance?.HidePlayCardPanel();
        GameManager.Instance?.ResetCharacterShader();

        if (!CanUseCard() || Camera.main == null)
        {
            DestroyDraggedCard();
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        
        if (hit.collider != null && (hit.collider.GetComponent<TileBehaviour>() != null || hit.collider.GetComponent<CharacterPrefabScript>() != null))
        {
            var characterOnMouse = GetCharacterOnMouse();
            if (characterOnMouse == null || DraggedActionCard.DraggedCard == null)
            {
                DestroyDraggedCard();
                return;
            }

            this.UseCard();
            DraggedActionCard.DraggedCard.GetComponent<UIEffectTweener>()?.PlayForward();
            LeanTween.move(DraggedActionCard.DraggedCard, Camera.main.WorldToScreenPoint(new Vector3(characterOnMouse.gameObject.transform.position.x, characterOnMouse.gameObject.transform.position.y + 0.25f)), 0.6f).setEaseInCirc();
            LeanTween.scale(DraggedActionCard.DraggedCard, new Vector3(0.3f, 0.3f, 1f), 0.6f).setEaseInCirc()
                .setOnComplete(DestroyDraggedCard);
        } else {
            DestroyDraggedCard();
        }
    }

    private static void DestroyDraggedCard()
    {
        if (DraggedActionCard.DraggedCard != null)
        {
            MonoBehaviour.Destroy(DraggedActionCard.DraggedCard.gameObject);
            DraggedActionCard.DraggedCard = null;
        }
    }
}

public static class DraggedActionCard
{
    public static GameObject DraggedCard;

    public static GameObject InstantiateCard(Card card){
        if (card == null || PrefabDatabase.Instance == null || PrefabDatabase.Instance.draggedCardPrefab == null ||
            BoardGameUiManager.Instance == null)
        {
            return null;
        }

        var go = MonoBehaviour.Instantiate(PrefabDatabase.Instance.draggedCardPrefab, BoardGameUiManager.Instance.transform);
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; 
        go.transform.position = (mousePosition);
        go.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        if (go.transform.childCount > 0)
        {
            Image spRenderer = go.transform.GetChild(0).gameObject.GetComponent<Image>();
            if (spRenderer != null)
            {
                spRenderer.sprite = card.image;
            }
        }
        if (go.transform.childCount > 1)
        {
            go.transform.GetChild(1).gameObject.GetComponentInChildren<TextMeshProUGUI>()?.SetText(card.name);
        }
        // spRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        // go.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
        return go;
    }
}
