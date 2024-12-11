using UnityEngine;
using UnityEngine.Video;

public class TerminaVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Asigna el VideoPlayer en el Inspector
    public Canvas canvasToHide;     // Canvas que se ocultará (asignar en el Inspector)
    public Canvas canvasMision;     // Canvas que se mostrará después (asignar en el Inspector)

    void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
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
            canvasToHide.gameObject.SetActive(false);
        }

        if (canvasMision != null)
        {
            canvasMision.gameObject.SetActive(true);
            Debug.Log("Canvas de misión mostrado al finalizar el video.");
        }
    }
}
