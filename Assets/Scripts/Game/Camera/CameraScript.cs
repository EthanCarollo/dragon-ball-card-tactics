using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour
{
    public static CameraScript Instance;
    public Camera camera;
    public Transform baseTarget;
    public CinemachineCamera cinemachineCamera;
    public CinemachinePositionComposer cinemachinePositionComposer;

    public void Awake()
    {
        Instance = this;
        baseTarget = cinemachineCamera.Target.TrackingTarget;
    }

    public void SetupTransformationCamera(BoardObject boardObject){
        LeanTween.value(this.gameObject, (float leanValue) => {
            cinemachineCamera.Lens.OrthographicSize = leanValue;
        }, cinemachineCamera.Lens.OrthographicSize, 4f, 1f).setEaseInOutCirc();
        cinemachineCamera.Target.TrackingTarget = boardObject.gameObject.transform;
        cinemachinePositionComposer.Composition.ScreenPosition.Set(0f, 0.03f);
    }

    public void SetupFightCamera(){
        LeanTween.value(this.gameObject, (float leanValue) => {
            cinemachineCamera.Lens.OrthographicSize = leanValue;
        }, cinemachineCamera.Lens.OrthographicSize, 5f, 1f).setEaseInOutCirc();
        cinemachineCamera.Target.TrackingTarget=baseTarget;
        cinemachinePositionComposer.Composition.ScreenPosition.Set(0f, 0.03f);
    }

    public void SetupNormalCamera(){
        LeanTween.value(this.gameObject, (float leanValue) => {
            cinemachineCamera.Lens.OrthographicSize = leanValue;
        }, cinemachineCamera.Lens.OrthographicSize, 7f, 1f).setEaseInOutCirc();
        cinemachineCamera.Target.TrackingTarget=baseTarget;
        cinemachinePositionComposer.Composition.ScreenPosition.Set(0f, 0f);
    }
}