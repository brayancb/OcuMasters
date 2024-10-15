using UnityEngine;

public class desaparecer : MonoBehaviour
{
    // Variable para almacenar el Canvas que quieres ocultar
    public GameObject canvas;

    // Función que será llamada cuando se haga clic en el botón
    public void Hide()
    {
        // Desactiva el Canvas
        canvas.SetActive(false);
    }
}
