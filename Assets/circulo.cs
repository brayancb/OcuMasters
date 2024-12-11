using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Video;
using UnityEngine.Events;
using System.Collections.Generic; // <-- Asegúrate de incluir esto

public class CircleProgress : MonoBehaviour
{
    public Material radialMaterial;
    public float holdTime = 2f;
    public GameObject canvasToHide;
    public Canvas canvasMision;     // Canvas que se mostrará después (asignar en el Inspector)
    public VideoPlayer videoPlayer;
    public UnityEvent onCanvasHidden;

    private InputDevice leftController;
    private InputDevice rightController;
    private float currentHoldTime = 0f;
    private bool isFilling = false;

    void Start()
    {
        InitializeControllers();
        ResetProgress();

        if (onCanvasHidden == null)
        {
            onCanvasHidden = new UnityEvent();
        }
    }

    void Update()
    {
        DetectTriggerHold();

        if (isFilling && radialMaterial != null)
        {
            currentHoldTime += Time.deltaTime;
            float progress = Mathf.Clamp01(currentHoldTime / holdTime);
            float arcValue = Mathf.Lerp(360f, 0f, progress);
            radialMaterial.SetFloat("_Arc1", arcValue);

            if (progress >= 1f)
            {
                TriggerCompleteAction();
                ResetProgress();
            }
        }
        else
        {
            ResetProgress();
        }
    }

    private void TriggerCompleteAction()
    {
        if (canvasToHide != null)
        {
            canvasToHide.SetActive(false);

            if (onCanvasHidden != null)
            {
                onCanvasHidden.Invoke();
            }
        }

        if (canvasMision != null)
        {
            canvasMision.gameObject.SetActive(true);
            Debug.Log("Canvas de misión mostrado al saltar con los gatillos.");
        }

        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }
    }

    private void ResetProgress()
    {
        currentHoldTime = 0f;
        if (radialMaterial != null)
        {
            radialMaterial.SetFloat("_Arc1", 360f);
        }
        isFilling = false;
    }

    private void DetectTriggerHold()
    {
        bool leftPressed = false;
        bool rightPressed = false;

        if (leftController.isValid)
            leftController.TryGetFeatureValue(CommonUsages.triggerButton, out leftPressed);

        if (rightController.isValid)
            rightController.TryGetFeatureValue(CommonUsages.triggerButton, out rightPressed);

        isFilling = leftPressed && rightPressed;
    }

    private void InitializeControllers()
    {
        var leftHanded = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHanded);
        if (leftHanded.Count > 0)
        {
            leftController = leftHanded[0];
        }

        var rightHanded = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHanded);
        if (rightHanded.Count > 0)
        {
            rightController = rightHanded[0];
        }
    }
}
