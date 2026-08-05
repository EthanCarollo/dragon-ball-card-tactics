using UnityEngine;

public class PlayableCardContextMenu : MonoBehaviour {
    private RectTransform rectTransform;
    private Card card;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (rectTransform == null)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            // Vérifie si la souris est en dehors du GameObject
            if (!IsPointerInside())
            {
                Destroy(gameObject);
            }
        }
    }

    private bool IsPointerInside()
    {
        // Convertit la position de la souris en coordonnées locales par rapport au RectTransform
        Vector2 localMousePosition = rectTransform.InverseTransformPoint(Input.mousePosition);
        return rectTransform.rect.Contains(localMousePosition);
    }

    public void SetupMenu(Card card){
        this.card = card;
    }

    public void RemoveCard(){
        if (card == null)
        {
            Destroy(gameObject);
            return;
        }

        GameManager.Instance.RemoveCard(card);
        GameManager.Instance.Player.Mana.AddMana(1);
        Destroy(gameObject);
    }
}
