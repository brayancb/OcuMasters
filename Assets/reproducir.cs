using System.Collections;
using UnityEngine;

public class CanvasExplicacion : MonoBehaviour
{
    public Canvas canvasExplicacionControles; // Este canvas (asignar en el Inspector).
    public Canvas canvasMision;              // Canvas que se mostrará (asignar en el Inspector).
    public float delay = 7f;                 // Tiempo antes de mostrar el Canvas de misión.

    private bool skipped = false;            // Indica si el jugador ha saltado el video.

    void Start()
    {
        // Desactivar el Canvas de misión al inicio.
        if (canvasMision != null)
        {
            canvasMision.gameObject.SetActive(false);
        }

        // Iniciar la espera para cambiar de canvas.
        StartCoroutine(SwitchToMissionCanvas());
    }

    void Update()
    {
        // Detectar si se presionan los gatillos para saltar.
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) // Asume "Fire1" y "Fire2" para gatillos.
        {
            skipped = true; // Marcar que se saltó manualmente.
            ShowMissionCanvas();
        }
    }

    private IEnumerator SwitchToMissionCanvas()
    {
        // Esperar el tiempo especificado (si no se salta).
        yield return new WaitForSeconds(delay);

        if (!skipped) // Si no se saltó manualmente.
        {
            ShowMissionCanvas();
        }
    }

    private void ShowMissionCanvas()
    {
        // Ocultar el canvas actual y mostrar el de misión.
        if (canvasExplicacionControles != null)
        {
            canvasExplicacionControles.gameObject.SetActive(false);
        }

        if (canvasMision != null)
        {
            canvasMision.gameObject.SetActive(true);
        }

        Debug.Log("Canvas de misión mostrado.");
    }
}
