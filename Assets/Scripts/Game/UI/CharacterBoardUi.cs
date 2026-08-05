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

        public bool showStars = true;
        public Transform starsContainer;
        public Sprite star;

        public RectTransform specialAttackContainerLayout;
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
                if (characterBoardUi == null)
                {
                        return;
                }

                if (characterContainer == null || characterContainer.GetCharacterData() == null)
                {
                        characterBoardUi.SetActive(false);
                        return;
                }
                characterBoardUi.SetActive(true);
                var characterData = characterContainer.GetCharacterData();
                if (charNameText != null) charNameText.text = characterContainer.GetName();
                if (charHealth != null)
                {
                        charHealth.maxValue = characterContainer.GetCharacterMaxHealth();
                        charHealth.value = characterContainer.actualHealth;
                }

                if (charKi != null)
                {
                        charKi.maxValue = characterData.maxKi;
                        charKi.value = characterContainer.actualKi;
                }
                
                if (charArmorText != null) charArmorText.text = "AR: " + characterContainer.GetArmor();
                if (charDamageText != null) charDamageText.text = "AD: " + characterContainer.GetAttackDamage();
                if (charCriticalText != null) charCriticalText.text = "CC: " + characterContainer.GetCriticalChance() + "%";
                if (charAttackSpeedText != null) charAttackSpeedText.text = "AS: " + characterContainer.GetAttackSpeed();
                
                if (charHealthText != null) charHealthText.text = characterContainer.actualHealth + " / " + characterContainer.GetCharacterMaxHealth();
                if (charKiText != null) charKiText.text = characterContainer.actualKi + " / " + characterContainer.GetCharacterMaxKi();
                if (charImage != null) charImage.sprite = characterData.characterIcon;

                specialAttackContainer?.Setup(characterContainer.GetCharacterSpecialAttack(), characterContainer);
                if (specialAttackContainerLayout != null) LayoutRebuilder.ForceRebuildLayoutImmediate(specialAttackContainerLayout);
                
                if(!string.IsNullOrEmpty(characterData.spriteCredit)){
                        spriteCreditContainer?.SetActive(true);
                        spriteCredit?.gameObject.SetActive(true);
                        if (spriteCredit != null)
                        {
                                spriteCredit.text = "Sprite Credits : " + characterData.spriteCredit;
                                spriteCredit.maskable = false;
                        }
                }else{
                        spriteCreditContainer?.SetActive(false);
                        spriteCredit?.gameObject.SetActive(false);
                }
                
                if (synergyContainer != null)
                {
                        foreach (Transform child in synergyContainer)
                        {
                                Destroy(child.gameObject);
                        }
                }
                var synergies = characterContainer.GetSynergies();
                if(synergyContainer != null && synergies != null && synergies.Length != 0){
                        synergyContainer.gameObject.SetActive(true);
                        foreach (var synergy in synergies)
                        {
                                if (synergy == null || synergyPrefab == null) continue;
                                Instantiate(synergyPrefab, synergyContainer).GetComponent<SynergyCharacterShowPrefabScript>()?.Setup(synergy);
                        }
                }else if (synergyContainer != null){
                        synergyContainer.gameObject.SetActive(false);
                }


                var alreadyCreatedPassive = new List<CharacterPassive>();
                if (passiveContainer != null)
                {
                        foreach (Transform child in passiveContainer)
                        {
                                var passiveContainerObject = child.gameObject.GetComponent<PassiveContainer>();
                                if(passiveContainerObject != null && characterContainer.GetCharacterAdditionalPassives() != null){
                                        if(characterContainer.GetCharacterAdditionalPassives().Contains(passiveContainerObject.passive)){
                                                alreadyCreatedPassive.Add(passiveContainerObject.passive);
                                                continue;
                                        }
                                }
                                Destroy(child.gameObject);
                        }
                }

                var additionalPassives = characterContainer.GetCharacterAdditionalPassives();
                if(additionalPassives != null){
                        foreach (var passive in additionalPassives){
                                if(passive == null || alreadyCreatedPassive.Contains(passive) ||
                                   passiveLittlePrefab == null || passiveContainer == null) continue;
                                
                                Instantiate(passiveLittlePrefab, passiveContainer).GetComponent<PassiveContainer>()?.Setup(passive);
                        }

                        passiveWholeContainer?.SetActive(additionalPassives.Length > 0);
                }

                if (starsContainer != null)
                {
                        foreach (Transform child in starsContainer)
                        {
                                Destroy(child.gameObject);
                        }
                }
                if (starsContainer != null && showStars && star != null)
                {
                        for (int i = 0; i < characterContainer.characterStar; i++)
                        {
                                var characterStar = new GameObject("CharacterStar");
                                characterStar.AddComponent<Image>().sprite = star;
                                characterStar.transform.SetParent(starsContainer, false);
                                characterStar.GetComponent<RectTransform>().sizeDelta = new Vector2(45, 45);
                        }
                }

                var defaultPassive = characterContainer.GetDefaultPassive();
                if (defaultCharacterPassiveGameObject == null)
                {
                        var layoutWithoutPassive = characterBoardUi.GetComponent<RectTransform>();
                        if (layoutWithoutPassive != null) LayoutRebuilder.ForceRebuildLayoutImmediate(layoutWithoutPassive);
                        return;
                }

                if (defaultPassive == null) {
                        defaultCharacterPassiveGameObject.SetActive(false);
                } else {
                        defaultCharacterPassiveGameObject.SetActive(true);
                        if (defaultCharacterPassiveImage != null) defaultCharacterPassiveImage.sprite = defaultPassive.passiveImage;
                        if (defaultCharacterPassiveText != null)
                        {
                                defaultCharacterPassiveText.text = defaultPassive.passiveName + "\n \n" + defaultPassive.GetDescription();
                                defaultCharacterPassiveText.maskable = false;
                        }
                }
                var layout = characterBoardUi.GetComponent<RectTransform>();
                if (layout != null) LayoutRebuilder.ForceRebuildLayoutImmediate(layout);
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
