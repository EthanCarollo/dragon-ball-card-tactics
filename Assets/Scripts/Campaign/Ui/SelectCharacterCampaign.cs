using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SelectCharacterCampaign : MonoBehaviour
{
    public GameObject selectCharacterPrefab;
    public GameObject selectCampaignContainer;

    public Transform selectCharacterParent;

    public static SelectCharacterCampaign Instance;
    [NonSerialized]
    public CampaignContainer campaign;

    public void Start(){
        Instance = this;
        selectCampaignContainer.SetActive(false);
    }

    public void SetupCampaign(CampaignContainer campaign){
        selectCampaignContainer.SetActive(true);
        foreach (Transform child in selectCharacterParent)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0; i < CharacterInventory.Instance.selectedIndexCharacterForCampaign.Length; i++){
            var go = Instantiate(selectCharacterPrefab, selectCharacterParent);
            go.GetComponent<SelectCharacterScript>().SetupCharacter(i);
        }
        this.campaign = campaign;
    }

    public void StartCampaign(){
        List<CharacterContainer> charContainerForFight = new List<CharacterContainer>();
        for(int i = 0; i < CharacterInventory.Instance.selectedIndexCharacterForCampaign.Length; i++){
            if(CharacterInventory.Instance.selectedIndexCharacterForCampaign[i] != -1){
                charContainerForFight.Add(CharacterInventory.Instance.characters[CharacterInventory.Instance.selectedIndexCharacterForCampaign[i]]);
            }
        }
        CampaignUtils.LaunchCampaign(campaign, charContainerForFight.ToArray());
    }
}