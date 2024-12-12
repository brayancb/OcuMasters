using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Video;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class CircleProgress : MonoBehaviour
{
    public Material radialMaterial;
    public float holdTime = 2f;
    public GameObject canvasToHide;
    public Canvas canvasMision;     // Canvas que se mostrará después (asignar en el Inspector)
    public VideoPlayer videoPlayer;
    public UnityEvent onCanvasHidden;
    public MissionController missionController; // Asignar en el Inspector

    private InputDevice leftController;
    private InputDevice rightController;
    private float currentHoldTime = 0f;
    private bool isFilling = false;
    private bool missionStarted = false;

    void Start()
    {
        InitializeControllers();
        ResetProgress();

        if (onCanvasHidden == null)
        {
            onCanvasHidden = new UnityEvent();
        }

        // Suscribirse al evento de finalización del video
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    void OnDestroy()
    {
        // Cancelar la suscripción al evento
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }

    void Update()
    {
        // Solo detectar la entrada si la misión no ha comenzado
        if (!missionStarted)
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
                    TriggerCompleteAction(); // Oculta el canvas o salta el video
                    ResetProgress(); // Reinicia el progreso
                }
            }
            else
            {
                // Reinicia si se suelta algún gatillo
                ResetProgress();
            }
        }
    }

    private void InitializeControllers()
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
        // Evitamos que se llame más de una vez
        if (missionStarted) return;

        missionStarted = true;

        // Oculta el canvas para que no moleste al jugador
        if (canvasToHide != null)
        {
            canvasToHide.SetActive(false);
            onCanvasHidden.Invoke(); // Dispara el evento cuando el Canvas se oculta
        }

        // Detener el video si está reproduciéndose
        if (videoPlayer != null && videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
        }

        // Mostrar el CanvasMision
        if (canvasMision != null)
        {
            canvasMision.gameObject.SetActive(true);
            Debug.Log("CanvasMision mostrado.");
        }
        else
        {
            Debug.LogWarning("canvasMision no está asignado en CircleProgress.");
        }

        // Iniciar la misión
        if (missionController != null)
        {
            missionController.StartMission();
            Debug.Log("Misión iniciada a través de CircleProgress.");
        }
        else
        {
            Debug.LogWarning("MissionController no está asignado en CircleProgress.");
        }
    }

    private void OnVideoEnd(VideoPlayer vp)
    {
        // Evitamos que se llame más de una vez
        if (missionStarted) return;

        missionStarted = true;

        // Oculta el canvas para que no moleste al jugador
        if (canvasToHide != null)
        {
            canvasToHide.SetActive(false);
            onCanvasHidden.Invoke(); // Dispara el evento cuando el Canvas se oculta
        }

        // Mostrar el CanvasMision
        if (canvasMision != null)
        {
            canvasMision.gameObject.SetActive(true);
            Debug.Log("CanvasMision mostrado al finalizar el video.");
        }
        else
        {
            Debug.LogWarning("canvasMision no está asignado en CircleProgress.");
        }

        // Iniciar la misión
        if (missionController != null)
        {
            missionController.StartMission();
            Debug.Log("Misión iniciada al finalizar el video.");
        }
        else
        {
            Debug.LogWarning("MissionController no está asignado en CircleProgress.");
        }
    }

    private void ResetProgress()
    {
        currentHoldTime = 0f;
        if (radialMaterial != null)
        {
            radialMaterial.SetFloat("_Arc1", 360f); // Reinicia el círculo a completo
        }
        isFilling = false;
    }
}