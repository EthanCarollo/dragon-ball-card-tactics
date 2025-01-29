
using UnityEngine;
using UnityEngine.EventSystems;

class OpenSceneButton : MonoBehaviour, IPointerClickHandler {
    public int sceneIndex;

    public void OnPointerClick(PointerEventData eventData)
    {
        SceneTransitor.Instance.LoadScene(sceneIndex);
    }
}