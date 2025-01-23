using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public abstract class UsableCharacterActionCard : Card
{
    public CharacterData characterFor;

    protected BoardCharacter GetCharacterOnMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null && (hit.collider.GetComponent<TileBehaviour>() != null || hit.collider.GetComponent<CharacterPrefabScript>() != null))
        {
            return GameManager.Instance.GetCharactersOnBoard()
                    .Where(cha => cha.character.isPlayerCharacter).ToList()
                    .Find(cha => cha.character.GetCharacterData() == characterFor || cha.character.GetCharacterData().sameCharacters.Contains(characterFor));   
        }
        return null;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        // CameraScript.Instance.SetupFightCamera();
    }
    
    public override void OnDrag(PointerEventData eventData)
    {
        /*
        if (GetCharacterOnMouse() != null)
        {
            var newMaterial = new Material(ShadersDatabase.Instance.outlineMaterial);
            newMaterial.color = Color.white;
            SpriteRenderer renderer =
                GetCharacterOnMouse().gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
            renderer.material = newMaterial;
            renderer.material.SetTexture("_MainTex", renderer.sprite.texture);
            BoardGameUiManager.Instance.ShowPlayCardPanel();
        }
        else
        {
            BoardGameUiManager.Instance.HidePlayCardPanel();
            GameManager.Instance.ResetCharacterShader();
        }
        */
        if (DraggedActionCard.DraggedCard == null)
        {
            Debug.Log("Drag a character");
            DraggedActionCard.DraggedCard = MonoBehaviour.Instantiate(BoardGameUiManager.Instance.draggedCardPrefab, BoardGameUiManager.Instance.transform);
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f; 
            DraggedActionCard.DraggedCard.transform.position = (mousePosition);
            DraggedActionCard.DraggedCard.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            Image spRenderer = DraggedActionCard.DraggedCard.transform.GetChild(0).gameObject.GetComponent<Image>();
            spRenderer.sprite = this.image;
            spRenderer.color = new Color(1f, 1f, 1f, 0.5f);
            DraggedActionCard.DraggedCard.gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
        }
        else
        {
            LeanTween.cancel(DraggedActionCard.DraggedCard);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            
            if (hit.collider != null && (hit.collider.GetComponent<TileBehaviour>() != null || hit.collider.GetComponent<CharacterPrefabScript>() != null))
            {
                BoardCharacter characterExist = GameManager.Instance.GetCharactersOnBoard()
                    .Where(cha => cha.character.isPlayerCharacter).ToList().Find(cha => cha.character.GetCharacterData() == characterFor || cha.character.GetCharacterData().sameCharacters.Contains(characterFor));
                if(characterExist != null){
                    var positionCharacter = BoardUtils.FindPosition(GameManager.Instance.boardCharacterArray, characterExist);
                    var characterUiPosition = Camera.main.WorldToScreenPoint(new Vector3(positionCharacter.x, positionCharacter.y + 2, 0f));
                    LeanTween.move(DraggedActionCard.DraggedCard, characterUiPosition, 0.1f).setEaseOutSine();
                    BoardGameUiManager.Instance.ShowPlayCardPanel();
                    return;
                }
            }
            BoardGameUiManager.Instance.HidePlayCardPanel();
            
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f; 
                LeanTween.move(DraggedActionCard.DraggedCard, new Vector3(mousePosition.x, mousePosition.y, 0f), 0.1f).setEaseOutSine();
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        BoardGameUiManager.Instance.HidePlayCardPanel();
        GameManager.Instance.ResetCharacterShader();
        if (DraggedActionCard.DraggedCard != null)
        {
            MonoBehaviour.Destroy(DraggedActionCard.DraggedCard.gameObject);
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        
        if (hit.collider != null && (hit.collider.GetComponent<TileBehaviour>() != null || hit.collider.GetComponent<CharacterPrefabScript>() != null))
        {
            this.UseCard();    
        }
    }
}

public static class DraggedActionCard
{
    public static GameObject DraggedCard;
}