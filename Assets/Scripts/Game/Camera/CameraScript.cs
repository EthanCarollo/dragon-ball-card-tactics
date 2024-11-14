using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    public static CameraScript Instance;
    public Camera camera;
    public CinemachineCamera cinemachineCamera;
    public CinemachinePositionComposer cinemachinePositionComposer;

    public void Awake()
    {
        Instance = this;
    }

    public void SetupFightCamera(){
        LeanTween.value(this.gameObject, (float leanValue) => {
            cinemachineCamera.Lens.OrthographicSize = leanValue;
        }, cinemachineCamera.Lens.OrthographicSize, 5f, 1f).setEaseInOutCirc();
        cinemachinePositionComposer.Composition.ScreenPosition.Set(0f, 0.03f);
    }

    public void SetupNormalCamera(){
        LeanTween.value(this.gameObject, (float leanValue) => {
            cinemachineCamera.Lens.OrthographicSize = leanValue;
        }, cinemachineCamera.Lens.OrthographicSize, 7f, 1f).setEaseInOutCirc();
        cinemachinePositionComposer.Composition.ScreenPosition.Set(0f, 0f);
    }
}