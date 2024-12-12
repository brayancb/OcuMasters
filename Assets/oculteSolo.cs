using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class OculteSolo : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Asigna el VideoPlayer en el Inspector
    public Canvas canvasToHide;     // Asigna el Canvas que deseas ocultar en el Inspector

    void Start()
    {
        if (videoPlayer != null)
        {
            // Suscribirse al evento que se dispara al terminar el video
            videoPlayer.loopPointReached += OnVideoEnd;
        }
        else
        {
            Debug.LogWarning("No se ha asignado un VideoPlayer a 'videoPlayer'.");
        }

        if (canvasToHide == null)
        {
            Debug.LogWarning("No se ha asignado un Canvas a 'canvasToHide'.");
        }
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            // Cancelar la suscripción al evento
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (canvasToHide != null)
        {
            // Ocultar el Canvas
            canvasToHide.gameObject.SetActive(false);
            Debug.Log("Canvas ocultado automáticamente al finalizar el video.");
        }
    }
}