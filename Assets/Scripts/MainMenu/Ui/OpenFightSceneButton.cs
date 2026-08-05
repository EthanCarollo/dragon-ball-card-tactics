
using UnityEngine;
using UnityEngine.EventSystems;

class OpenFightSceneButton : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData)
    {
        if (SceneTransitor.Instance == null)
        {
            Debug.LogError("Cannot open the fight scene: SceneTransitor is missing.");
            return;
        }

        SceneTransitor.Instance.LoadScene(1, () =>
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.Start();
            }
        });
    }
}
