
using UnityEngine;
using UnityEngine.EventSystems;

class OpenFightSceneButton : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData)
    {
        SceneTransitor.Instance.LoadScene(1, () => {
            GameManager.Instance.Start();
        });
    }
}