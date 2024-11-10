using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas; // El canvas del menú
    public GameObject controlsCanvas; // El canvas que explica los controles
    public Button startButton; // El botón de "Comenzar"
    public float delayBeforeShowingControls = 10f; // Tiempo de espera antes de mostrar los controles
    public float fadeDuration = 2f; // Duración del fade-in de los controles

    void Start()
    {
        // Agrega las dos funciones al evento OnClick del botón
        startButton.onClick.AddListener(HideMenuCanvas);    // Oculta el menú
        startButton.onClick.AddListener(ActivateControlsCanvas);  // Activa el canvas de controles
    }

    // Función para ocultar el menú
    public void HideMenuCanvas()
    {
        menuCanvas.SetActive(false); // Oculta el canvas del menú
    }

    // Función para activar el canvas de controles
    public void ActivateControlsCanvas()
    {
        controlsCanvas.SetActive(true); // Activa el canvas de los controles
        StartCoroutine(ShowControlsWithDelay()); // Inicia el proceso de fade-in
    }

    // Corutina que aplica el delay y el fade-in para el canvas de controles
    private IEnumerator ShowControlsWithDelay()
    {
        // Espera el tiempo especificado antes de mostrar los controles
        yield return new WaitForSeconds(delayBeforeShowingControls);

        // Obtiene el CanvasGroup del canvas de los controles
        CanvasGroup canvasGroup = controlsCanvas.GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f; // Inicialmente invisible
            float timer = 0f;

            // Realiza el fade-in del canvas de los controles
            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
                yield return null;
            }
            canvasGroup.alpha = 1f; // Finalmente completamente visible
        }
    }
}
