using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas; // El canvas del menú
    public GameObject controlsCanvas; // El canvas que explica los controles
    public Button startButton; // El botón de "Comenzar"
    public float delayBeforeShowingControls = 10f; // Tiempo de espera antes de mostrar los controles
    public float fadeDuration = 2f; // Duración del fade-in de los controles

    private InputDevice leftController;
    private InputDevice rightController;
    private float triggerHoldTime = 0f;
    private float requiredHoldTime = 2f; // Tiempo requerido de 2 segundos

    void Start()
    {
        // Agrega las dos funciones al evento OnClick del botón
        startButton.onClick.AddListener(HideMenuCanvas);    // Oculta el menú
        startButton.onClick.AddListener(ActivateControlsCanvas);  // Activa el canvas de controles
        InitializeControllers();
    }

    void InitializeControllers()
    {
        var leftHandedControllers = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandedControllers);
        if (leftHandedControllers.Count > 0)
            leftController = leftHandedControllers[0];

        var rightHandedControllers = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandedControllers);
        if (rightHandedControllers.Count > 0)
            rightController = rightHandedControllers[0];
    }

    void Update()
    {
        CheckTriggers();
    }

    void CheckTriggers()
    {
        bool leftPressed = false;
        bool rightPressed = false;

        if (leftController.isValid)
            leftController.TryGetFeatureValue(CommonUsages.triggerButton, out leftPressed);

        if (rightController.isValid)
            rightController.TryGetFeatureValue(CommonUsages.triggerButton, out rightPressed);

        if (leftPressed && rightPressed)
        {
            triggerHoldTime += Time.deltaTime;
            if (triggerHoldTime >= requiredHoldTime)
            {
                // Actúa como si se hubiera presionado el botón de inicio
                HideMenuCanvas();
                ActivateControlsCanvas();
                triggerHoldTime = 0f; // Reinicia el contador
            }
        }
        else
        {
            triggerHoldTime = 0f; // Reinicia si se suelta algún gatillo
        }
    }

    // Función para ocultar el menú
    public void HideMenuCanvas()
    {
        menuCanvas.SetActive(false); // Oculta el canvas del menú
    }

    // Función para activar el canvas de controles
    public void ActivateControlsCanvas()
    {
        controlsCanvas.SetActive(true); // Activa el canvas de los controles
        StartCoroutine(ShowControlsWithDelay()); // Inicia el proceso de fade-in
    }

    // Corutina que aplica el delay y el fade-in para el canvas de controles
    private IEnumerator ShowControlsWithDelay()
    {
        // Espera el tiempo especificado antes de mostrar los controles
        yield return new WaitForSeconds(delayBeforeShowingControls);

        // Obtiene el CanvasGroup del canvas de los controles
        CanvasGroup canvasGroup = controlsCanvas.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f; // Inicialmente invisible
            float timer = 0f;

            // Realiza el fade-in del canvas de los controles
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                yield return null;
            }
            canvasGroup.alpha = 1f; // Finalmente completamente visible
        }
    }
}