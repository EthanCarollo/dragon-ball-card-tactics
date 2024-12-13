using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SynergyCharacterShowPrefabScript : MonoBehaviour {
    public Synergy synergy;
    public TextMeshProUGUI synergyNumber;
    public Image synergyImage;

    public void Setup(Synergy synergy) {
        this.synergy = synergy;
        synergyNumber.text = synergy.synergyName;
        synergyImage.sprite = synergy.synergyImage;
    }

}