using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class SelectCharacterScript : MonoBehaviour, IPointerClickHandler {
    public Image behindContainer;
    public Image character;

    public int index;
    public int characterIndex = -1;

    public void SetupCharacter(int selectIndex){
        index = selectIndex;
        SelectCharacter(CharacterInventory.Instance.selectedIndexCharacterForCampaign[index]);
    }

    // This logics shouldn't be there, but in a problem of prototyping, I made this to be more fast
    public void SelectCharacter(int characterIndexInInventory){
        if(characterIndexInInventory == -1){
            characterIndex = -1;
            behindContainer.color = Color.white;
            character.gameObject.SetActive(false);
            CharacterInventory.Instance.selectedIndexCharacterForCampaign[index] = characterIndexInInventory;
            return;
        }
        if(CharacterInventory.Instance.selectedIndexCharacterForCampaign.Contains(characterIndexInInventory) && 
            CharacterInventory.Instance.selectedIndexCharacterForCampaign[index] != characterIndexInInventory){
            return;
        }
        characterIndex = characterIndexInInventory;
        character.sprite = CharacterInventory.Instance.characters[characterIndexInInventory].GetCharacterData().characterIcon;
        character.gameObject.SetActive(true);
        behindContainer.color = CharacterInventory.Instance.characters[characterIndexInInventory].GetCharacterData().GetCharacterColor();
        CharacterInventory.Instance.selectedIndexCharacterForCampaign[index] = characterIndexInInventory;
    }
    
    public void OnPointerClick(PointerEventData pointerEventData){
        var newIndex = characterIndex+1;
        if(newIndex >= CharacterInventory.Instance.characters.Count){
            newIndex = -1;
        }
        while(CharacterInventory.Instance.selectedIndexCharacterForCampaign.Contains(newIndex) && newIndex != -1){
            newIndex++;
            if(newIndex >= CharacterInventory.Instance.characters.Count){
                newIndex = -1;
            }
        }
        SelectCharacter(newIndex);
    }
}