using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBoardUi : MonoBehaviour
{
        private BoardCharacter boardCharacter;
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
        public SpecialAttackContainer specialAttackContainer;

        public GameObject passiveWholeContainer;
        public GameObject passiveLittlePrefab;
        public Transform passiveContainer;

        public void Update()
        {
                if (boardCharacter == null)
                {
                        characterBoardUi.gameObject.SetActive(false);
                        return;
                }
                characterBoardUi.gameObject.SetActive(true);
                charNameText.text = boardCharacter.character.GetCharacterData().name;
                charHealth.maxValue = boardCharacter.character.GetCharacterData().maxHealth;
                charHealth.value = boardCharacter.character.actualHealth;
                
                charKi.maxValue = boardCharacter.character.GetCharacterData().maxKi;
                charKi.value = boardCharacter.character.actualKi;
                
                charArmorText.text = "AR: " + boardCharacter.character.GetArmor().ToString();
                charDamageText.text = "AD: " + boardCharacter.character.GetAttackDamage().ToString();
                charCriticalText.text = "CC: " + boardCharacter.character.GetCharacterData().baseCriticalChance.ToString() + "%";
                charAttackSpeedText.text = "AS: " + boardCharacter.character.GetAttackSpeed().ToString();
                
                charHealthText.text = boardCharacter.character.actualHealth + " / " + boardCharacter.character.GetCharacterData().maxHealth;
                charKiText.text = boardCharacter.character.actualKi + " / " + boardCharacter.character.GetCharacterData().maxKi;
                charImage.sprite = boardCharacter.character.GetCharacterData().characterIcon;

                specialAttackContainer.Setup(boardCharacter.character.GetCharacterSpecialAttack());

                foreach (Transform child in passiveContainer)
                {
                        Destroy(child.gameObject);
                }

                if(boardCharacter.character.GetCharacterPassives() != null){
                        foreach (var passive in boardCharacter.character.GetCharacterPassives()){
                                if(passive == null) continue;
                                Instantiate(passiveLittlePrefab, passiveContainer).GetComponent<PassiveContainer>().Setup(passive);
                        }

                        if(boardCharacter.character.GetCharacterPassives().Length > 0) {
                                passiveWholeContainer.SetActive(true);
                        } else {
                                passiveWholeContainer.SetActive(false);
                        }
                }
        }

        public void ShowCharacterBoard(BoardCharacter character)
        {
                boardCharacter = character;
        }
}