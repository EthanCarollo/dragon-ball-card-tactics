using UnityEngine;

public static class GameObjectExtensions
{
    // Extension method to swap the active state of a GameObject
    public static void SwapActive(this GameObject go)
    {
        // Toggle the active state (if it's active, it becomes inactive and vice versa)
        go.SetActive(!go.activeSelf);
    }
}
