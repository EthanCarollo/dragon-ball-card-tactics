using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
        public TextMeshProUGUI dialogText;
        public GameObject dialogBox;
        public Image leftCharacterImage;
        public Image rightCharacterImage;
        
        public Dialog[] dialogList;
        public int actualDialog = 0;
        public static DialogManager Instance;

        public void Awake()
        {
                Instance = this;
        }
        
        public void SetupDialog(Dialog[] dialogList)
        {
                this.dialogList = dialogList;
                dialogBox.SetActive(true);
                if (actualDialog >= dialogList.Length)
                {
                        dialogBox.SetActive(false);
                        return;
                }
                SetupTextDialog();
        }

        public void GoNextDialog()
        {
                actualDialog++;
                if (actualDialog >= dialogList.Length)
                {
                        dialogBox.SetActive(false);
                        return;
                }
                SetupTextDialog();
        }

        public void SetupTextDialog()
        {
                var dialog = dialogList[actualDialog];
                dialogText.text = dialog.content;
                if (dialog.leftCharacter is not null)
                {
                        leftCharacterImage.gameObject.SetActive(true);
                        leftCharacterImage.sprite = dialog.leftCharacter.characterIcon;
                        if (dialog.leftTalk)
                        {
                                LeanTween.scale(leftCharacterImage.gameObject.GetComponent<RectTransform>(), new Vector3(1.2f,1.2f,1.2f), 0.2f);
                        }
                        else
                        {
                                LeanTween.scale(leftCharacterImage.gameObject.GetComponent<RectTransform>(), new Vector3(1f,1f,1f), 0.2f);
                        }
                }
                else
                {
                        leftCharacterImage.gameObject.SetActive(false);
                }
                
                if (dialog.rightCharacter is not null)
                {
                        rightCharacterImage.gameObject.SetActive(true);
                        rightCharacterImage.sprite = dialog.rightCharacter.characterIcon;
                        if (!dialog.leftTalk)
                        {
                                LeanTween.scale(rightCharacterImage.gameObject.GetComponent<RectTransform>(), new Vector3(-1.2f,1.2f,1.2f), 0.2f);
                        }
                        else
                        {
                                LeanTween.scale(rightCharacterImage.gameObject.GetComponent<RectTransform>(), new Vector3(-1f,1f,1f), 0.2f);
                        }
                }
                else
                {
                        rightCharacterImage.gameObject.SetActive(false);
                }
        }
}