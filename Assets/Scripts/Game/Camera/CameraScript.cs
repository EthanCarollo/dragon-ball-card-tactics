using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    public static CameraScript Instance;
    public new Camera camera;
    public Transform baseTarget;
    public CinemachineCamera cinemachineCamera;
    public CinemachinePositionComposer cinemachinePositionComposer;

    public void Awake()
    {
        Instance = this;
        baseTarget = cinemachineCamera.Target.TrackingTarget;
    }

    public void SetupFightCamera(){
        LeanTween.value(this.gameObject, (float leanValue) => {
            cinemachineCamera.Lens.OrthographicSize = leanValue;
        }, cinemachineCamera.Lens.OrthographicSize, 4.5f, 0.5f).setEaseInOutCirc();
        cinemachineCamera.Target.TrackingTarget=baseTarget;
        cinemachinePositionComposer.Composition.ScreenPosition.Set(0f, 0.03f);
    }

    public void SetupNormalCamera(){
        LeanTween.value(this.gameObject, (float leanValue) => {
            cinemachineCamera.Lens.OrthographicSize = leanValue;
        }, cinemachineCamera.Lens.OrthographicSize, 5.5f, 0.5f).setEaseInOutCirc();
        cinemachineCamera.Target.TrackingTarget=baseTarget;
        cinemachinePositionComposer.Composition.ScreenPosition.Set(0f, 0f);
    }

    public void SetupCameraOnTarget(float size, Transform target)
    {
        LeanTween.value(this.gameObject, (float leanValue) => {
            cinemachineCamera.Lens.OrthographicSize = leanValue;
        }, cinemachineCamera.Lens.OrthographicSize, size, 0.5f).setEaseInOutCirc();
        cinemachineCamera.Target.TrackingTarget=baseTarget;
        cinemachineCamera.Target.TrackingTarget = target;
        cinemachinePositionComposer.Composition.ScreenPosition.Set(0f, 0f);
    }
}