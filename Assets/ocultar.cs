using UnityEngine;
using UnityEngine.Video;

public class OcultarCanvas : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Asigna el VideoPlayer en el Inspector
    public Canvas canvasToHide;     // Asigna el Canvas en el Inspector

    void Start()
    {
        if (videoPlayer != null)
        {
            // Suscribirse al evento loopPointReached del VideoPlayer
            videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    void OnDestroy()
    {
        if (videoPlayer != null)
        {
            // Cancelar la suscripción al evento loopPointReached
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (canvasToHide != null)
        {
            // Ocultar el Canvas
            canvasToHide.gameObject.SetActive(false);
        }
    }
}