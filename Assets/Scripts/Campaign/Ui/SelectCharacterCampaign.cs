using System;
using UnityEngine;


public class SelectCharacterCampaign : MonoBehaviour
{
    public GameObject selectCharacterPrefab;
    public GameObject selectCampaignContainer;

    public Transform selectCharacterParent;

    public void Start(){
        SetupCampaign();
    }

    public void SetupCampaign(){
        foreach (Transform child in selectCharacterParent)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0; i < CharacterInventory.Instance.selectedIndexCharacterForCampaign.Length; i++){
            var go = Instantiate(selectCharacterPrefab, selectCharacterParent);
            go.GetComponent<SelectCharacterScript>().SetupCharacter(i);
        }
    }
}