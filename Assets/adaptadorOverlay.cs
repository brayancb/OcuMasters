using UnityEngine;

public class HUDFollower : MonoBehaviour
{
    public Transform cameraTransform; // La cámara del jugador
    public float distanceFromCamera = 0.5f; // Distancia del HUD a la cámara

    private void Update()
    {
        if (cameraTransform != null)
        {
            // Mantener el HUD enfrente de la cámara
            transform.position = cameraTransform.position + cameraTransform.forward * distanceFromCamera;
            transform.rotation = cameraTransform.rotation; // Siempre orientado hacia el jugador
        }
    }
}
