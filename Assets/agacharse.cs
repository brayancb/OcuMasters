using UnityEngine;
using UnityEngine.XR;

public class UpdateCharacterController : MonoBehaviour
{
    public CharacterController characterController;
    public Transform xrCamera;  // La cámara de la XR Rig, que representa la cabeza del jugador

    private void Update()
    {
        // Ajusta la altura del CharacterController en función de la posición de la cámara
        characterController.height = xrCamera.localPosition.y;

        // Mantén el centro del CharacterController en el medio del cuerpo del jugador
        Vector3 center = characterController.center;
        center.y = characterController.height / 2;  // Ajusta el centro a la mitad de la altura
        characterController.center = center;
    }
}
