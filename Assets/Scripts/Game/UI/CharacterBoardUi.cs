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
                charNameText.text = boardCharacter.character.name;
                charHealth.maxValue = boardCharacter.character.maxHealth;
                charHealth.value = boardCharacter.actualHealth;
                
                charKi.maxValue = boardCharacter.character.maxKi;
                charKi.value = boardCharacter.actualKi;
                
                charArmorText.text = boardCharacter.GetArmor().ToString();
                charDamageText.text = boardCharacter.GetAttackDamage().ToString();
                charSpeedText.text = boardCharacter.GetSpeed().ToString();
                charAttackSpeedText.text = boardCharacter.GetAttackSpeed().ToString();
                
                charStarText.text = boardCharacter.character.starNumber + " star";
                
                charHealthText.text = boardCharacter.actualHealth + " / " + boardCharacter.character.maxHealth;
                charKiText.text = boardCharacter.actualKi + " / " + boardCharacter.character.maxKi;
                charImage.sprite = boardCharacter.character.characterSprite;
        }

        public void ShowCharacterBoard(BoardCharacter character)
        {
                boardCharacter = character;
        }
}