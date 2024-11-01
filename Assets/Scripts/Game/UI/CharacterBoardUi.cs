using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBoardUi : MonoBehaviour
{
        private BoardCharacter boardCharacter;
        public GameObject characterBoardUi;
        public TextMeshProUGUI charStarText;
        public TextMeshProUGUI charNameText;
        public TextMeshProUGUI charArmorText;
        public TextMeshProUGUI charDamageText;
        public TextMeshProUGUI charSpeedText;
        public TextMeshProUGUI charAttackSpeedText;
        public Slider charHealth;
        public TextMeshProUGUI charHealthText;
        public Slider charKi;
        public TextMeshProUGUI charKiText;
        public Image charImage;

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
                charHealth.value = boardCharacter.actualHealth;
                
                charKi.maxValue = boardCharacter.character.GetCharacterData().maxKi;
                charKi.value = boardCharacter.actualKi;
                
                charArmorText.text = boardCharacter.character.GetArmor().ToString();
                charDamageText.text = boardCharacter.character.GetAttackDamage().ToString();
                charSpeedText.text = boardCharacter.character.GetSpeed().ToString();
                charAttackSpeedText.text = boardCharacter.character.GetAttackSpeed().ToString();
                
                charStarText.text = boardCharacter.character.GetCharacterData().starNumber + " star";
                
                charHealthText.text = boardCharacter.actualHealth + " / " + boardCharacter.character.GetCharacterData().maxHealth;
                charKiText.text = boardCharacter.actualKi + " / " + boardCharacter.character.GetCharacterData().maxKi;
                charImage.sprite = boardCharacter.character.GetCharacterData().characterSprite;
        }

        public void ShowCharacterBoard(BoardCharacter character)
        {
                boardCharacter = character;
        }
}