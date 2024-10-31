using UnityEngine;

public class SaveMenuUi : MonoBehaviour
{
        public GameObject saveMenuButton;

        public void Start()
        {
                SaveButton button = Instantiate(saveMenuButton, this.transform).GetComponent<SaveButton>();
                button.Setup();
        }
}