using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SelectCharacterCampaign : MonoBehaviour
{
    public GameObject selectCharacterPrefab;
    public GameObject simpleCharacterContainerPrefab;
    public GameObject selectCampaignContainer;
    public TextMeshProUGUI campaignTitle;

    public Transform selectCharacterParent;
    public Transform enemyCharacterParent;

    public static SelectCharacterCampaign Instance;
    [NonSerialized]
    public CampaignContainer campaign;

    public void Start(){
        Instance = this;
        selectCampaignContainer.SetActive(false);
    }

    public void SetupCampaign(CampaignContainer campaign){
        CharacterInventory.Instance.VerifyCharacterSelected();
        campaignTitle.text = campaign.GetActualCampaign().campaignName;
        selectCampaignContainer.SetActive(true);
        foreach (Transform child in selectCharacterParent)
        {
            Destroy(child.gameObject);
        }

        for(int i = 0; i < CharacterInventory.Instance.selectedIndexCharacterForCampaign.Length; i++){
            var go = Instantiate(selectCharacterPrefab, selectCharacterParent);
            go.GetComponent<SelectCharacterScript>().SetupCharacter(i);
        }

        foreach (Transform child in enemyCharacterParent)
        {
            Destroy(child.gameObject);
        }

        int charAdded = 0;

        foreach (var level in campaign.GetActualCampaign().levels)
        {
            for(int i = 0; i < level.characters.Length; i++){
                if (charAdded >= 3)
                {
                    break;
                }
                var go = Instantiate(simpleCharacterContainerPrefab, enemyCharacterParent);
                go.GetComponent<CharacterSimpleContainer>().SetupCharacter(level.characters[i].character);
                charAdded++;
            }
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
        if(charContainerForFight.Count == 0){
            Debug.LogWarning("PROBLEM : There is no character in the character fight array");
            return;
        }
        CampaignUtils.LaunchCampaign(campaign, charContainerForFight.ToArray());
    }
}