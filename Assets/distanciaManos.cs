// distanciaManos.cs
using UnityEngine;

public class HandOffset : MonoBehaviour
{
    public Transform leftHand;  // Asigna la mano izquierda en el editor
    public Transform rightHand; // Asigna la mano derecha en el editor
    public Vector3 offset = new Vector3(0, 0, 0.2f); // Ajusta el desplazamiento según sea necesario

    void Start()
    {
        // Aplica el desplazamiento inicial a las manos
        if (leftHand != null)
            leftHand.localPosition += offset;

        if (rightHand != null)
            rightHand.localPosition += offset;
    }
}