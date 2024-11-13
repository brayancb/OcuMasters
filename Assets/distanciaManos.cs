using UnityEngine;

public class HandOffset : MonoBehaviour
{
    public Transform leftHand;  // Asigna manualmente el objeto de la mano izquierda desde el editor
    public Transform rightHand; // Asigna manualmente el objeto de la mano derecha desde el editor

    void Update()
    {
        // Mueve las manos hacia adelante en el eje Z local
        leftHand.localPosition += new Vector3(0, 0, 6.6f);  // Ajusta el valor "0.2f" según tu necesidad
        rightHand.localPosition += new Vector3(0, 0, 6.6f); // Ajusta el valor "0.2f" según tu necesidad

    }

    
}
