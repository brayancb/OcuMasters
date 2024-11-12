using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class RandomWander : MonoBehaviour
{
    // Configuración de movimiento
    public float speed = 3f;               // Velocidad de movimiento
    public float stopDurationMin = 1f;     // Tiempo mínimo de parada en segundos
    public float stopDurationMax = 4f;     // Tiempo máximo de parada en segundos
    public float wanderRange = 10f;        // Radio de movimiento aleatorio

    private Vector3 targetPosition;        // Siguiente posición objetivo
    private bool isMoving = false;         // Estado de movimiento

    void Start()
    {
        SetRandomDestination();
    }

    void Update()
    {
        if (isMoving)
        {
            // Mueve el objeto hacia el punto objetivo
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            // Verifica si ha llegado al punto objetivo
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                StartCoroutine(StopAndSetNewDestination());
            }
        }
    }

    // Asigna un nuevo destino aleatorio dentro del rango de movimiento
    void SetRandomDestination()
    {
        float randomX = Random.Range(-wanderRange, wanderRange);
        float randomZ = Random.Range(-wanderRange, wanderRange);
        targetPosition = new Vector3(randomX, transform.position.y, randomZ);
        isMoving = true;
    }

    // Corrutina para detenerse y luego moverse a un nuevo destino
    IEnumerator StopAndSetNewDestination()
    {
        isMoving = false;
        
        // Tiempo de espera aleatorio
        float stopDuration = Random.Range(stopDurationMin, stopDurationMax);
        yield return new WaitForSeconds(stopDuration);

        // Asigna un nuevo destino y reanuda el movimiento
        SetRandomDestination();
    }
}
