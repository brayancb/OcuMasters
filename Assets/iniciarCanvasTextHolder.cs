using UnityEngine;
using UnityEngine.Playables;

public class IniciarCanvasTextHolder : MonoBehaviour
{
    public PlayableDirector cinematicDirector; // Asigna el PlayableDirector en el Inspector
    public Canvas canvasToShow;                // Asigna el Canvas a mostrar en el Inspector

    void Start()
    {
        if (cinematicDirector != null)
        {
            // Suscribirse al evento que se dispara al terminar la cinemática
            cinematicDirector.stopped += OnCinematicEnded;
            Debug.Log("Suscrito al evento 'stopped' del PlayableDirector.");
        }
        else
        {
            Debug.LogWarning("cinematicDirector no está asignado en el Inspector.");
        }

        if (canvasToShow != null)
        {
            // Ocultar el Canvas al inicio
            canvasToShow.gameObject.SetActive(false);
            Debug.Log("Canvas ocultado al inicio.");
        }
        else
        {
            Debug.LogWarning("canvasToShow no está asignado en el Inspector.");
        }
    }

    void OnDestroy()
    {
        if (cinematicDirector != null)
        {
            // Cancelar la suscripción al evento
            cinematicDirector.stopped -= OnCinematicEnded;
            Debug.Log("Desuscrito del evento 'stopped' del PlayableDirector.");
        }
    }

    void OnCinematicEnded(PlayableDirector director)
    {
        Debug.Log("Evento 'stopped' del PlayableDirector disparado.");
        if (canvasToShow != null)
        {
            // Mostrar el Canvas
            canvasToShow.gameObject.SetActive(true);
            Debug.Log("Canvas mostrado.");
        }
        else
        {
            Debug.LogWarning("canvasToShow es nulo en OnCinematicEnded.");
        }
    }
}