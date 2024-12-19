using System;
using TMPro;
using UnityEngine;

public class FightNameUi : MonoBehaviour
{
        public GameObject fightPanel;
        public TextMeshProUGUI fightNameText;
        public TextMeshProUGUI fightDifficultyText;

        private void Start()
        {
                ExitFightNamePanel();
        }

        public void OpenFightNamePanel(Fight fight)
        {
                fightPanel.GetComponent<RectTransform>().anchoredPosition = 
                        new Vector2(fightPanel.GetComponent<RectTransform>().sizeDelta.x, 0f);
                fightPanel.SetActive(true);
                fightNameText.text = fight.name;
                fightDifficultyText.text = "Difficulty : " + fight.difficulty.ToString();

                fightPanel.GetComponent<RectTransform>().localScale = Vector3.zero;
                fightPanel.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, -15);

                LeanTween.scale(fightPanel, Vector3.one, 0.5f).setEaseOutBack();
                LeanTween.rotateZ(fightPanel, 0f, 0.5f).setEaseOutBack();
                LeanTween.moveX(fightPanel.GetComponent<RectTransform>(), 0f, 0.5f).setEaseOutBack()
                        .setOnComplete(() =>
                        {
                                // Animation de pulsation
                                LeanTween.scale(fightPanel, new Vector3(1.05f, 1.05f, 1.05f), 0.5f)
                                        .setEasePunch()
                                        .setLoopPingPong(1);

                                // Animation de sortie
                                LeanTween.moveX(fightPanel.GetComponent<RectTransform>(), -fightPanel.GetComponent<RectTransform>().sizeDelta.x, 0.5f)
                                        .setEaseInBack()
                                        .setDelay(2f)
                                        .setOnComplete(() =>
                                        {
                                                LeanTween.delayedCall(0.5f, () => { ExitFightNamePanel(); });
                                        });
                        });

                fightNameText.color = new Color(fightNameText.color.r, fightNameText.color.g, fightNameText.color.b, 0);
                fightDifficultyText.color = new Color(fightDifficultyText.color.r, fightDifficultyText.color.g, fightDifficultyText.color.b, 0);

                LeanTween.value(fightPanel, 0f, 1f, 0.5f)
                        .setDelay(0.25f)
                        .setOnUpdate((float val) =>
                        {
                                fightNameText.color = new Color(fightNameText.color.r, fightNameText.color.g, fightNameText.color.b, val);
                        });

                LeanTween.value(fightPanel, 0f, 1f, 0.5f)
                        .setDelay(0.5f)
                        .setOnUpdate((float val) =>
                        {
                                fightDifficultyText.color = new Color(fightDifficultyText.color.r, fightDifficultyText.color.g, fightDifficultyText.color.b, val);
                        });
        }

        public void ExitFightNamePanel()
        {
                fightPanel.SetActive(false);
        }
}