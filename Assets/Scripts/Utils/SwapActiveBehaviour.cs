using UnityEngine;
using UnityEngine.EventSystems;

public class SwapActiveBehaviour : MonoBehaviour
{
    public void SwapActive(GameObject go)
    {
        go.SwapActive();
    }
}
