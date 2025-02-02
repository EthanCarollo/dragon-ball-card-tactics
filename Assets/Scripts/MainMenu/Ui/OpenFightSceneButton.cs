
using UnityEngine;
using UnityEngine.EventSystems;

class OpenFightSceneButton : MonoBehaviour, IPointerClickHandler {
    public void OnPointerClick(PointerEventData eventData)
    {
        SceneTransitor.Instance.LoadScene(2, () => {
            GameManager.Instance.Start();
        });
    }
}