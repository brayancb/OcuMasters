using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustAlpacaPosition : MonoBehaviour
{
    public float yOffset = 1.0f; // Ajuste hacia arriba del modelo

    void Start()
    {
        // Ajustar la posición Y del modelo de la alpaca
        Vector3 newPosition = transform.position;
        newPosition.y += yOffset;
        transform.position = newPosition;
    }
}
