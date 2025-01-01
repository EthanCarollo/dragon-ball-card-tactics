using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "TransformationCard", menuName = "Card/TransformationCard")]
public class TransformationCard : Card
{
    [SerializeField]
    public TransformationsPossible[] transformations;

    protected BoardCharacter GetCharacterOnMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        
        if (hit.collider != null && 
            (hit.collider.GetComponent<TileBehaviour>() != null || hit.collider.GetComponent<CharacterPrefabScript>() != null))
        {
            return GameManager.Instance.GetCharactersOnBoard()
                .Where(cha => cha.isPlayerCharacter)
                .ToList()
                .Find(cha => {
                    // Vérifie si le CharacterData de cha.character est dans transformations
                    return transformations.Any(trans => trans.character == cha.character.GetCharacterData());
                });
        }
        return null;
    }

    public override bool CanUseCard()
    {
        // Cherche un personnage sur le plateau dont le CharacterData est dans transformations
        var characterOnBoard = GameManager.Instance.GetCharactersOnBoard()
            .Where(cha => cha.isPlayerCharacter)
            .ToList()
            .Find(cha =>
                transformations.Any(trans => trans.character == cha.character.GetCharacterData())
            );

        // Si aucun personnage correspondant n'est trouvé, on ne peut pas utiliser la carte
        if (characterOnBoard == null)
        {
            return false;
        }

        // Si un personnage est trouvé, on appelle la méthode de base pour vérifier d'autres conditions
        return base.CanUseCard();
    }


    public override void UseCard()
    {
        if (CanUseCard() == false)
        {
            return;
        }

        // Vérifie si un personnage correspondant est sous la souris
        BoardCharacter targetCharacter = GetCharacterOnMouse();

        // Log de débogage pour le personnage sélectionné
        Debug.LogWarning(targetCharacter != null 
            ? $"Target character found: {targetCharacter.character.GetCharacterData().name}" 
            : "No target character found.");

        if (targetCharacter != null)
        {
            // Trouve la transformation correspondante pour le CharacterData du personnage
            var transformation = transformations.FirstOrDefault(trans => trans.character == targetCharacter.character.GetCharacterData());

            if (transformation != null)
            {
                // Joue l'animation de transformation pour le personnage
                targetCharacter.PlayAnimation(transformation.transformation);
            }

            // Réduit le mana du joueur
            GameManager.Instance.Player.Mana.CurrentMana -= manaCost;

            // Met à jour l'UI pour refléter la perte de mana
            BoardGameUiManager.Instance.ShowLooseMana(manaCost);
            BoardGameUiManager.Instance.RefreshUI();

            // Retire la carte après utilisation
            GameManager.Instance.RemoveCard(this);
        }
    }


    public override void OnBeginDrag(PointerEventData eventData)
    {
        
    }
    
    public override void OnDrag(PointerEventData eventData)
    {
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
            
            if (hit.collider != null && 
                (hit.collider.GetComponent<TileBehaviour>() != null || hit.collider.GetComponent<CharacterPrefabScript>() != null))
            {
                // Cherche un personnage sur le plateau qui correspond à un des personnages dans 'transformations'
                BoardCharacter characterExist = GameManager.Instance.GetCharactersOnBoard()
                    .Where(cha => cha.isPlayerCharacter)
                    .ToList()
                    .Find(cha => 
                        transformations.Any(trans => trans.character == cha.character.GetCharacterData())
                    );

                if (characterExist != null)
                {
                    // Trouve la position du personnage sur le plateau
                    var positionCharacter = BoardUtils.FindPosition(GameManager.Instance.boardCharacterArray, characterExist);
                    var characterUiPosition = Camera.main.WorldToScreenPoint(new Vector3(positionCharacter.x, positionCharacter.y + 2, 0f));

                    // Déplace la carte vers la position du personnage
                    LeanTween.move(DraggedActionCard.DraggedCard, characterUiPosition, 0.1f).setEaseOutSine();

                    // Affiche le panneau "Play Card"
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

[Serializable]
public class TransformationsPossible {
    public CharacterData character;
    public TransformAnimation transformation;
}