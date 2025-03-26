using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBoardUi : MonoBehaviour
{
        private CharacterContainer characterContainer;
        public GameObject characterBoardUi;
        public TextMeshProUGUI charNameText;
        public TextMeshProUGUI charArmorText;
        public TextMeshProUGUI charDamageText;
        public TextMeshProUGUI charCriticalText;
        public TextMeshProUGUI charAttackSpeedText;
        public Slider charHealth;
        public TextMeshProUGUI charHealthText;
        public Slider charKi;
        public TextMeshProUGUI charKiText;
        public Image charImage;

        public Transform synergyContainer;
        public GameObject synergyPrefab;

        public GameObject passiveWholeContainer;
        public GameObject passiveLittlePrefab;
        public Transform passiveContainer;

        public Transform starsContainer;
        public Sprite star;

        public SpecialAttackContainer specialAttackContainer;

        public GameObject spriteCreditContainer;
        public TextMeshProUGUI spriteCredit;

        public GameObject defaultCharacterPassiveGameObject;
        public Image defaultCharacterPassiveImage;
        public TextMeshProUGUI defaultCharacterPassiveText;

        public void Start()
        {
                RefreshUi();
        }

        public void RefreshUi()
        {
                if (characterContainer == null)
                {
                        characterBoardUi.gameObject.SetActive(false);
                        return;
                }
                characterBoardUi.gameObject.SetActive(true);
                charNameText.text = characterContainer.GetName();
                charHealth.maxValue = characterContainer.GetCharacterMaxHealth();
                charHealth.value = characterContainer.actualHealth;
                
                charKi.maxValue = characterContainer.GetCharacterData().maxKi;
                charKi.value = characterContainer.actualKi;
                
                charArmorText.text = "AR: " + characterContainer.GetArmor().ToString();
                charDamageText.text = "AD: " + characterContainer.GetAttackDamage().ToString();
                charCriticalText.text = "CC: " + characterContainer.GetCriticalChance().ToString() + "%";
                charAttackSpeedText.text = "AS: " + characterContainer.GetAttackSpeed().ToString();
                
                charHealthText.text = characterContainer.actualHealth + " / " + characterContainer.GetCharacterMaxHealth();
                charKiText.text = characterContainer.actualKi + " / " + characterContainer.GetCharacterMaxKi();
                charImage.sprite = characterContainer.GetCharacterData().characterIcon;

                specialAttackContainer.Setup(characterContainer.GetCharacterSpecialAttack());

                if(characterContainer.GetCharacterData().spriteCredit != null && characterContainer.GetCharacterData().spriteCredit != ""){
                        spriteCreditContainer.SetActive(true);
                        spriteCredit.gameObject.SetActive(true);
                        spriteCredit.text = "Sprite Credits : " + characterContainer.GetCharacterData().spriteCredit;
                        spriteCredit.maskable = false;
                }else{
                        spriteCreditContainer.SetActive(false);
                        spriteCredit.gameObject.SetActive(false);
                }
                
                foreach (Transform child in synergyContainer)
                {
                        Destroy(child.gameObject);
                }
                if(characterContainer.GetSynergies() != null && characterContainer.GetSynergies().Length != 0){
                        synergyContainer.gameObject.SetActive(true);
                        foreach (var synergy in characterContainer.GetSynergies())
                        {
                                Instantiate(synergyPrefab, synergyContainer).GetComponent<SynergyCharacterShowPrefabScript>().Setup(synergy);
                        }
                }else{
                        synergyContainer.gameObject.SetActive(false);
                }


                var alreadyCreatedPassive = new List<CharacterPassive>();
                foreach (Transform child in passiveContainer)
                {
                        var passiveContainer = child.gameObject.GetComponent<PassiveContainer>();
                        if(passiveContainer != null && characterContainer.GetCharacterAdditionalPassives() != null){
                                if(characterContainer.GetCharacterAdditionalPassives().Contains(passiveContainer.passive)){
                                        alreadyCreatedPassive.Add(passiveContainer.passive);
                                        continue;
                                }
                        } 
                        Destroy(child.gameObject);
                }

                if(characterContainer.GetCharacterAdditionalPassives() != null){
                        foreach (var passive in characterContainer.GetCharacterAdditionalPassives()){
                                if(passive == null || alreadyCreatedPassive.Contains(passive)) continue;
                                
                                Instantiate(passiveLittlePrefab, passiveContainer).GetComponent<PassiveContainer>().Setup(passive);
                        }

                        if(characterContainer.GetCharacterAdditionalPassives().Length > 0) {
                                passiveWholeContainer.SetActive(true);
                        } else {
                                passiveWholeContainer.SetActive(false);
                        }
                }
                
                foreach (Transform child in starsContainer)
                {
                        Destroy(child.gameObject);
                }
                for (int i = 0; i < characterContainer.characterStar; i++)
                {
                        var characterStar = new GameObject();
                        characterStar.AddComponent<Image>().sprite = star;
                        characterStar.AddComponent<RectTransform>();
                        characterStar.GetComponent<RectTransform>().sizeDelta = new Vector2(45, 45);
                        Instantiate(characterStar, starsContainer);
                }

                if (characterContainer.GetDefaultPassive() == null) {
                        defaultCharacterPassiveGameObject.SetActive(false);
                } else {
                        defaultCharacterPassiveGameObject.SetActive(true);
                        defaultCharacterPassiveImage.sprite = characterContainer.GetDefaultPassive().passiveImage;
                        defaultCharacterPassiveText.text = characterContainer.GetDefaultPassive().passiveName + "\n \n" + characterContainer.GetDefaultPassive().passiveDescription;
                        defaultCharacterPassiveText.maskable = false;
                }
        }

        public void ShowCharacterBoard(CharacterContainer character)
        {
                if (characterContainer != null)
                {
                        characterContainer.OnCharacterChanged -= RefreshUi;
                }

                characterContainer = character;

                if (characterContainer != null)
                {
                        characterContainer.OnCharacterChanged += RefreshUi;
                }
                RefreshUi();
        }

        public void HideCharacterBoard()
        {
                if (characterContainer != null)
                {
                        characterContainer.OnCharacterChanged -= RefreshUi;
                }

                characterContainer = null;
                RefreshUi();
        }
}