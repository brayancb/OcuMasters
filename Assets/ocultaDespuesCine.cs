using System.Collections;
using UnityEngine;

public class OcultarCanvasDespuesDeTiempo : MonoBehaviour
{
    public Canvas canvasToHide; // Asigna el Canvas que deseas ocultar
    public float displayTime = 5f; // Tiempo en segundos que el Canvas estará visible

    void Start()
    {
        if (canvasToHide != null)
        {
            // Asegurarse de que el Canvas esté activo al inicio
            canvasToHide.gameObject.SetActive(true);
            // Iniciar la corrutina para ocultar el Canvas después del tiempo especificado
            StartCoroutine(HideCanvasAfterTime());
        }
        else
        {
            Debug.LogWarning("No se ha asignado un Canvas a 'canvasToHide'.");
        }
    }

    private IEnumerator HideCanvasAfterTime()
    {
        // Esperar el tiempo especificado
        yield return new WaitForSeconds(displayTime);

        // Ocultar el Canvas
        canvasToHide.gameObject.SetActive(false);
        Debug.Log("Canvas ocultado después de " + displayTime + " segundos.");
    }
}