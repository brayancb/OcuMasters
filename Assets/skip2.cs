using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Video;
using UnityEngine.UI;

public class SkipVideoOnTrigger : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Asigna el VideoPlayer en el Inspector
    public RawImage rawImage;       // Asigna el RawImage en el Inspector
    public float holdTime = 2f;     // Tiempo para completar el círculo

    private InputDevice leftController;
    private InputDevice rightController;
    private float currentHoldTime = 0f;
    private bool isFilling = false;

    void Start()
    {
        InitializeControllers();

        if (videoPlayer != null && rawImage != null)
        {
            // Crear una RenderTexture y asignarla al VideoPlayer y al RawImage
            RenderTexture renderTexture = new RenderTexture(1920, 1080, 0);
            videoPlayer.targetTexture = renderTexture;
            rawImage.texture = renderTexture;
        }
    }

    void InitializeControllers()
    {
        var leftHanded = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHanded);
        if (leftHanded.Count > 0)
            leftController = leftHanded[0];

        var rightHanded = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHanded);
        if (rightHanded.Count > 0)
            rightController = rightHanded[0];
    }

    void Update()
    {
        DetectTriggerHold();
    }

    private void DetectTriggerHold()
    {
        bool leftPressed = false;
        bool rightPressed = false;

        if (leftController.isValid)
            leftController.TryGetFeatureValue(CommonUsages.triggerButton, out leftPressed);

        if (rightController.isValid)
            rightController.TryGetFeatureValue(CommonUsages.triggerButton, out rightPressed);

        // Si ambos gatillos están presionados
        isFilling = leftPressed && rightPressed;

        if (isFilling)
        {
            currentHoldTime += Time.deltaTime;
            if (currentHoldTime >= holdTime)
            {
                SkipVideo();
                ResetProgress();
            }
        }
        else
        {
            ResetProgress();
        }
    }

    private void SkipVideo()
    {
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
            Debug.Log("Video skipped");
        }
    }

    private void ResetProgress()
    {
        currentHoldTime = 0f;
        isFilling = false;
    }
}