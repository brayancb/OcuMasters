using UnityEngine;

public class HandOffset : MonoBehaviour
{
    public Transform leftHandModel;  // Asigna el modelo de la mano izquierda
    public Transform rightHandModel; // Asigna el modelo de la mano derecha
    public Vector3 offset = new Vector3(0, 0, 0.1f); // Ajusta el desplazamiento según sea necesario

    void Update()
    {
        // Aplica el desplazamiento al modelo de la mano izquierda
        if (leftHandModel != null)
            leftHandModel.localPosition = offset;

        // Aplica el desplazamiento al modelo de la mano derecha
        if (rightHandModel != null)
            rightHandModel.localPosition = offset;
    }
}