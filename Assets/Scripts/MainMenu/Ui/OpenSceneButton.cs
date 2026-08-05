
using UnityEngine;
using UnityEngine.EventSystems;

class OpenSceneButton : MonoBehaviour, IPointerClickHandler {
    public int sceneIndex;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (SceneTransitor.Instance == null)
        {
            Debug.LogError("Cannot open the scene: SceneTransitor is missing.");
            return;
        }

        SceneTransitor.Instance.LoadScene(sceneIndex);
    }
}
