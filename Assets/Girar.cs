using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateImage : MonoBehaviour
{
    public float rotationSpeed = 100f; // Velocidad de rotación

    void Update()
    {
        // Rotar el GameObject en el eje Y
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
