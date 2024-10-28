using System;
using UnityEngine;

public class SceneTransitor : MonoBehaviour
{
    [NonSerialized]
    public SceneTransitor Instance;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(this);
    }
}
