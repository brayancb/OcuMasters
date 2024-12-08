using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public Transform playerCamera; // Asigna el HMD del jugador (generalmente, la Main Camera)
    public TextMeshProUGUI floatingText; // Asigna tu texto flotante

    void Update()
    {
        // Posiciona el texto frente al jugador
        Vector3 targetPosition = playerCamera.position + playerCamera.forward * 2f;
        transform.position = targetPosition;

        // Asegura que el texto siempre apunte al jugador
        transform.rotation = Quaternion.LookRotation(transform.position - playerCamera.position);
    }
}
