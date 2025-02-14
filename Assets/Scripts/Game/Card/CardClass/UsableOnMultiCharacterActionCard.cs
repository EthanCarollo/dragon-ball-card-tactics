using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using System.Collections.Generic;
using Coffee.UIEffects;

// Todo : Rename the file into TransformationCard.cs
[CreateAssetMenu(fileName = "TransformationCard", menuName = "Card/TransformationCard")]
public class TransformationCard : Card
{
    [SerializeField]
    public TransformationsPossible[] transformations;
    public bool infiniteUse = false;

    protected BoardCharacter GetCharacterOnMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        if (hit.collider != null && 
            (hit.collider.GetComponent<TileBehaviour>() != null || hit.collider.GetComponent<CharacterPrefabScript>() != null))
        {
            var charactersUpdatable = GameManager.Instance.GetCharactersOnBoard()
                .Where(cha => cha.character.isPlayerCharacter)
                .ToList()
                .FindAll(cha => {
                    // Vérifie si le CharacterData de cha.character est dans transformations
                    return transformations.Any(trans => trans.character == cha.character.GetCharacterData());
                });
            if(charactersUpdatable.Count == 0) return null;

            var mousePosInWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Debug.LogWarning("Mouse position = " + mousePosInWorld);

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

    public override string GetDescription()
    {
        return "Transform " + transformations[0].character.characterName + " into " + transformations[0].transformation.newCharacterData.characterName + " or in several other possibilities.";
    }

    public override bool CanUseCard()
    {
        
        var characterOnBoard = GameManager.Instance.GetCharactersOnBoard()
            .Where(cha => cha.character.isPlayerCharacter)
            .ToList()
            .Find(cha =>
                transformations.Any(trans => trans.character == cha.character.GetCharacterData())
            );

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
            LeanTween.delayedCall(0.5f, () =>
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
                if(infiniteUse == false) GameManager.Instance.RemoveCard(this);
            });
        }
    }


    public override void OnBeginDrag(PointerEventData eventData)
    {
        
    }
    
    BoardCharacter actualSelectCharacter = null;
    
    public override void OnDrag(PointerEventData eventData)
    {
        if (DraggedActionCard.DraggedCard == null)
        {
            DraggedActionCard.DraggedCard = DraggedActionCard.InstantiateCard(this);
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
                    .Where(cha => cha.character.isPlayerCharacter)
                    .ToList()
                    .Find(cha => 
                        transformations.Any(trans => trans.character == cha.character.GetCharacterData())
                    );

                if (characterExist != null)
                {
                    if(actualSelectCharacter != null && actualSelectCharacter != characterExist){
                        try {
                            actualSelectCharacter.gameObject.GetComponentInChildren<CharacterPrefabScript>()?.ResetMaterial();
                        } catch (Exception error){
                            Debug.LogError(error);
                        }
                        actualSelectCharacter = null;
                    }
                    actualSelectCharacter = characterExist;
                    // Trouve la position du personnage sur le plateau
                    var positionCharacter = BoardUtils.FindPosition(GameManager.Instance.boardCharacterArray, characterExist);
                    var characterUiPosition = Camera.main.WorldToScreenPoint(new Vector3(positionCharacter.x, positionCharacter.y + 2, 0f));

                    // Déplace la carte vers la position du personnage
                    LeanTween.move(DraggedActionCard.DraggedCard, characterUiPosition, 0.1f).setEaseOutSine();

                    // Affiche le panneau "Play Card"
                    BoardGameUiManager.Instance.ShowPlayCardPanel();

                    if (characterExist != null && characterExist.character.IsDead() == false) {
                        try {
                            characterExist.gameObject.GetComponentInChildren<CharacterPrefabScript>()?.ApplyOutlineMaterial(Color.white);
                        } catch (Exception error){
                            Debug.LogError(error);
                        }
                    }
                    return;
                }
            } else {
                if(actualSelectCharacter != null){
                    try {
                        actualSelectCharacter.gameObject.GetComponentInChildren<CharacterPrefabScript>()?.ResetMaterial();
                    } catch (Exception error){
                        Debug.LogError(error);
                    }
                    actualSelectCharacter = null;
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

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
        
        if (hit.collider != null && (hit.collider.GetComponent<TileBehaviour>() != null || hit.collider.GetComponent<CharacterPrefabScript>() != null))
        {
            this.UseCard();
            DraggedActionCard.DraggedCard.GetComponent<UIEffectTweener>().PlayForward();
            LeanTween.move(DraggedActionCard.DraggedCard, Camera.main.WorldToScreenPoint(new Vector3(GetCharacterOnMouse().gameObject.transform.position.x, GetCharacterOnMouse().gameObject.transform.position.y + 0.25f)), 0.6f).setEaseInCirc();
            LeanTween.scale(DraggedActionCard.DraggedCard, new Vector3(0.3f, 0.3f, 1f), 0.6f).setEaseInCirc()
            .setOnComplete(() => { 
                if (DraggedActionCard.DraggedCard != null)
                {
                    MonoBehaviour.Destroy(DraggedActionCard.DraggedCard.gameObject);
                }
            });
        } else {
            if (DraggedActionCard.DraggedCard != null)
            {
                MonoBehaviour.Destroy(DraggedActionCard.DraggedCard.gameObject);
            }
        }
    }
}

[Serializable]
public class TransformationsPossible {
    public CharacterData character;
    public TransformAnimation transformation;
}