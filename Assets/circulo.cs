using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class CircleProgress : MonoBehaviour
{
    public Material radialMaterial; // El material que controla el círculo
    public float holdTime = 2f; // Tiempo para completar el círculo
    public GameObject canvasToHide; // El canvas que se ocultará

    private InputDevice leftController;
    private InputDevice rightController;
    private float currentHoldTime = 0f;
    private bool isFilling = false;

    void Start()
    {
        InitializeControllers();
        ResetProgress();
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

    private void Update()
    {
        DetectTriggerHold();

        // Si ambos gatillos están presionados, actualiza el círculo
        if (isFilling && radialMaterial != null)
        {
            currentHoldTime += Time.deltaTime;
            float progress = Mathf.Clamp01(currentHoldTime / holdTime); // Normaliza el progreso entre 0 y 1

            // Calcula el nuevo valor de _Arc1
            float arcValue = Mathf.Lerp(360f, 0f, progress); // De 360 a 0 basado en el progreso
            radialMaterial.SetFloat("_Arc1", arcValue);

            // Si el progreso se completa, dispara la acción
            if (progress >= 1f)
            {
                TriggerCompleteAction(); // Oculta el canvas
                ResetProgress(); // Reinicia el progreso
            }
        }
        else
        {
            // Reinicia si se suelta algún gatillo
            ResetProgress();
        }
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
    }

    private void TriggerCompleteAction()
    {
        // Oculta el canvas para que no moleste al jugador
        if (canvasToHide != null)
        {
            canvasToHide.SetActive(false);
        }
    }

    private void ResetProgress()
    {
        currentHoldTime = 0f;
        if (radialMaterial != null)
        {
            radialMaterial.SetFloat("_Arc1", 360f); // Reinicia el círculo a completo
        }
    }
}