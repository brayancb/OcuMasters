using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{
    public Transform playerCamera; // Cámara principal del VR
    public float followDistance = 3f; // Distancia mínima para detenerse
    public float stopDistance = 1f; // Distancia para detener el movimiento
    public float rotationSpeed = 5f; // Velocidad de rotación de la alpaca
    public float heightOffset = 1f; // Ajuste de altura para evitar que se quede pegada al piso

    private NavMeshAgent agent; // Componente NavMeshAgent
    private Animator animator; // Componente Animator

    void Start()
    {
        // Obtener el componente NavMeshAgent
        agent = GetComponent<NavMeshAgent>();

        // Obtener el componente Animator
        animator = GetComponent<Animator>();

        // Verificar si la cámara principal está asignada
        if (playerCamera == null && Camera.main != null)
        {
            playerCamera = Camera.main.transform; // Asignar la cámara principal automáticamente
        }

        // Configurar algunas opciones iniciales del NavMeshAgent
        agent.stoppingDistance = stopDistance; // Ajustar la distancia de parada
        agent.updateRotation = false; // No rotar automáticamente con el NavMeshAgent

        // Ajustar la altura del agente
        agent.baseOffset = heightOffset; // Esto ajusta la altura del NavMeshAgent
    }

    void Update()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("No se asignó la cámara del jugador al script.");
            return;
        }

        // Calcular la distancia entre la alpaca y la cámara principal (jugador)
        float distance = Vector3.Distance(transform.position, playerCamera.position);

        // Mover hacia el jugador si está a más de la distancia de seguimiento
        if (distance > followDistance)
        {
            agent.isStopped = false;  // Reactivar el movimiento del NavMeshAgent
            agent.SetDestination(playerCamera.position);  // Establecer la posición del jugador como destino

            // Activar la animación de caminar si el agente se está moviendo
            if (agent.velocity.sqrMagnitude > 0.1f) // Solo caminar si el agente se mueve
            {
                animator.SetBool("IsWalking", true);
            }

            // Asegurarse de que la velocidad del NavMeshAgent sea la normal mientras camina
            agent.speed = 3.5f;  // Ajusta la velocidad de la alpaca mientras camina
        }
        else
        {
            // Detenerse si está cerca del jugador
            agent.isStopped = true;  // Detener el movimiento del NavMeshAgent
            agent.SetDestination(transform.position);  // Establecer la posición actual como destino para evitar que se mueva

            // Activar la animación de idle
            animator.SetBool("IsWalking", false);

            // Asegurarse de que la alpaca se quede quieta
            agent.velocity = Vector3.zero; // Esto asegura que el agente no se mueva
            agent.speed = 0f;  // Poner la velocidad en 0 para que no se mueva
        }

        // Rotación hacia el jugador (suavemente)
        Vector3 direction = (playerCamera.position - transform.position).normalized;
        direction.y = 0;  // Ignorar la componente Y para evitar que gire en el eje vertical
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        // Asegurarse de que la alpaca se mantenga sobre el terreno usando un raycast
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            // Ajustar la posición de la alpaca si se encuentra por debajo del terreno
            transform.position = new Vector3(transform.position.x, hit.point.y + heightOffset, transform.position.z);
        }
    }
}
