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
                        if(passiveContainer != null && characterContainer.GetCharacterPassives() != null){
                                if(characterContainer.GetCharacterPassives().Contains(passiveContainer.passive)){
                                        alreadyCreatedPassive.Add(passiveContainer.passive);
                                        continue;
                                }
                        } 
                        Destroy(child.gameObject);
                }

                if(characterContainer.GetCharacterPassives() != null){
                        foreach (var passive in characterContainer.GetCharacterPassives()){
                                if(passive == null || alreadyCreatedPassive.Contains(passive)) continue;
                                
                                Instantiate(passiveLittlePrefab, passiveContainer).GetComponent<PassiveContainer>().Setup(passive);
                        }

                        if(characterContainer.GetCharacterPassives().Length > 0) {
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
        }

        public void ShowCharacterBoard(CharacterContainer character)
        {
                if (characterContainer != null)
                {
                        // Unsubscribe from the previous character's event
                        characterContainer.OnCharacterChanged -= RefreshUi;
                }

                characterContainer = character;

                if (characterContainer != null)
                {
                        // Subscribe to the new character's event
                        characterContainer.OnCharacterChanged += RefreshUi;
                }

                RefreshUi();
        }

        public void HideCharacterBoard()
        {
                if (characterContainer != null)
                {
                        // Unsubscribe from the current character's event
                        characterContainer.OnCharacterChanged -= RefreshUi;
                }

                characterContainer = null;
                RefreshUi();
        }
}