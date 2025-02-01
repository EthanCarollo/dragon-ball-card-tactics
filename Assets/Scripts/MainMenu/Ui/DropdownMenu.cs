using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DropdownMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject menuPanel; // Panel contenant les boutons du menu
    public List<Button> menuButtons; // Liste des boutons du menu
    public List<GameObject> objectsToActivate; // Liste des GameObjects liés aux boutons

    private bool isMouseOver = false;
    private bool isMenuOpen = false;

    private void Start()
    {
        menuPanel.SetActive(false); // Cache le menu au début

        // Associe chaque bouton à un GameObject à activer
        for (int i = 0; i < menuButtons.Count; i++)
        {
            int index = i; // Empêche le problème de closure
            menuButtons[i].onClick.AddListener(() => ActivateOnlyObject(index));
        }
    }

    private void Update()
    {
        // Vérifie si la souris quitte la zone et ferme le menu
        if (isMenuOpen && !isMouseOver && !RectTransformUtility.RectangleContainsScreenPoint(menuPanel.GetComponent<RectTransform>(), Input.mousePosition))
        {
            CloseMenu();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
        OpenMenu();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }

    private void OpenMenu()
    {
        menuPanel.SetActive(true);
        isMenuOpen = true;
    }

    private void CloseMenu()
    {
        menuPanel.SetActive(false);
        isMenuOpen = false;
    }

    private void ActivateOnlyObject(int index)
    {
        // Désactive tous les objets
        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(false);
        }

        // Active uniquement l'objet sélectionné
        if (index < objectsToActivate.Count)
        {
            objectsToActivate[index].SetActive(true);
        }

        // Ferme le menu après activation
        CloseMenu();
    }
}
