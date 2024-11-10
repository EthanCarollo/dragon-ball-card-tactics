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

    public void SetupCharacter(int selectIndex){
        index = selectIndex;
    }

    // This logics shouldn't be there, but in a problem of prototyping, I made this to be more fast
    public void SelectCharacter(int characterIndexInInventory){
        if(characterIndexInInventory == -1){
            CharacterInventory.Instance.selectedIndexCharacterForCampaign[index] = characterIndexInInventory;
            return;
        }
        if(CharacterInventory.Instance.selectedIndexCharacterForCampaign.Contains(characterIndexInInventory)){
            return;
        }
        character.sprite = CharacterInventory.Instance.characters[characterIndexInInventory].GetCharacterData().characterIcon;
        behindContainer.color = CharacterInventory.Instance.characters[characterIndexInInventory].GetCharacterData().GetCharacterColor();
        CharacterInventory.Instance.selectedIndexCharacterForCampaign[index] = characterIndexInInventory;
    }
    
    public void OnPointerClick(PointerEventData pointerEventData){
        SelectCharacter(1);
        Debug.Log("clicked");
    }
}