using UnityEngine;
using UnityEngine.XR;

public class CirculoController : MonoBehaviour
{
    public Material radialMaterial; // Material que utiliza el shader RadialFill
    public GameObject canvasHUD; // Canvas anclado a la cámara (HUD)
    private InputDevice leftController;
    private InputDevice rightController;

    private float holdTime = 2f; // Tiempo necesario para completar el círculo (2 segundos)
    private float currentHoldTime = 0f; // Tiempo actual manteniendo los gatillos
    private bool isFilling = false; // Para controlar si ambos gatillos están presionados

    private void Start()
    {
        InitializeControllers();

        // Configura el material inicial
        if (radialMaterial != null)
        {
            radialMaterial.SetFloat("_Arc1", 360f); // Empieza el círculo completo
            radialMaterial.SetFloat("_Arc2", 0f);
        }
    }

    private void InitializeControllers()
    {
        var leftHanded = new System.Collections.Generic.List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHanded);
        if (leftHanded.Count > 0)
            leftController = leftHanded[0];

        var rightHanded = new System.Collections.Generic.List<InputDevice>();
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
                TriggerCompleteAction(); // Aquí defines qué pasa cuando el círculo se llena
                ResetProgress(); // Reinicia el progreso
            }
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
        if (leftPressed && rightPressed)
        {
            isFilling = true;
        }
        else
        {
            isFilling = false;
            ResetProgress();
        }
    }

    private void TriggerCompleteAction()
    {
        Debug.Log("¡Círculo completado! Acción activada.");
        
        // Si deseas ocultar el Canvas al completar:
        if (canvasHUD != null)
        {
            canvasHUD.SetActive(false);
        }

        // Aquí puedes añadir más acciones, como iniciar el juego.
    }

    private void ResetProgress()
    {
        currentHoldTime = 0f;
        if (radialMaterial != null)
        {
            radialMaterial.SetFloat("_Arc1", 360f); // Reinicia el círculo
        }
    }
}
