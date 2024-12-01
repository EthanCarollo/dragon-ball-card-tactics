using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public abstract class UsableActionCard : Card
{
    protected abstract bool CanUseAction();
    public override void OnDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null)
        {
            TileBehaviour tileScript = hit.collider.GetComponent<TileBehaviour>();

            if (tileScript != null)
            {
                BoardGameUiManager.Instance.ShowPlayCardPanel();
            }
            else
            {
                BoardGameUiManager.Instance.HidePlayCardPanel();
            }
        }
        else
        {
            BoardGameUiManager.Instance.HidePlayCardPanel();
        }
        
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
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = 10f; 
            DraggedActionCard.DraggedCard.transform.position = (mousePosition);
        }
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (DraggedActionCard.DraggedCard != null)
        {
            MonoBehaviour.Destroy(DraggedActionCard.DraggedCard.gameObject);
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        
        if (hit.collider != null)
        {
            TileBehaviour tileScript = hit.collider.GetComponent<TileBehaviour>();

            if (tileScript != null)
            {
                BoardGameUiManager.Instance.HidePlayCardPanel();
                this.UseCard();
            }
            else
            {
                BoardGameUiManager.Instance.HidePlayCardPanel();
            }
        }
        else
        {
            BoardGameUiManager.Instance.HidePlayCardPanel();
        }
    }
}

public static class DraggedActionCard
{
    public static GameObject DraggedCard;
}