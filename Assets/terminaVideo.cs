using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class TerminaVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Asigna el VideoPlayer en el Inspector
    public Canvas canvasToHide;     // Canvas que se ocultará (asignar en el Inspector)
    public Canvas canvasMision;     // Canvas que se mostrará después (asignar en el Inspector)
    public float delay = 5f;        // Tiempo en segundos antes de mostrar el CanvasMision

    void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
        }

        // Asegurarse de que el canvasMision esté desactivado al inicio
        if (canvasMision != null)
        {
            canvasMision.gameObject.SetActive(false);
        }
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (canvasToHide != null)
        {
            // Ocultar el Canvas actual
            canvasToHide.gameObject.SetActive(false);
        }

        // Iniciar la corrutina para mostrar el CanvasMision después del retraso
        StartCoroutine(ShowCanvasMisionAfterDelay());
    }

    private IEnumerator ShowCanvasMisionAfterDelay()
    {
        // Esperar el tiempo de retraso
        yield return new WaitForSeconds(delay);

        if (canvasMision != null)
        {
            // Mostrar el CanvasMision
            canvasMision.gameObject.SetActive(true);
        }
    }
}