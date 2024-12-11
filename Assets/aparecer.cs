using System.Collections;
using UnityEngine;

public class ShowCanvasAfterTime : MonoBehaviour
{
    public Canvas canvasToShow; // Asigna el Canvas a mostrar
    public float delay = 5f;    // Tiempo en segundos antes de mostrar el Canvas
void Start()
{
    if (canvasToShow != null)
    {
        canvasToShow.gameObject.SetActive(false); // Ocultar el Canvas al inicio
        Debug.Log("Canvas ocultado al inicio.");
        StartCoroutine(ShowCanvasCoroutine());
    }
    else
    {
        Debug.LogWarning("canvasToShow no está asignado en el Inspector.");
    }
}

IEnumerator ShowCanvasCoroutine()
{
    Debug.Log("Iniciando corrutina para mostrar el Canvas después de " + delay + " segundos.");
    yield return new WaitForSeconds(delay);
    canvasToShow.gameObject.SetActive(true); // Mostrar el Canvas después del retraso
    Debug.Log("Canvas debería estar visible ahora.");
}
}