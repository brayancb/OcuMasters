using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FoxMovement : MonoBehaviour
{
    public Transform playerCamera;
    public float followDistance = 3f;
    public float stopDistance = 1f;
    public float rotationSpeed = 5f;
    public float heightOffset = 1f;
    public float runSpeed = 5f;

    private NavMeshAgent agent;
    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        if (playerCamera == null && Camera.main != null)
        {
            playerCamera = Camera.main.transform;
        }

        // Configuración del NavMeshAgent
        agent.stoppingDistance = stopDistance;
        agent.updateRotation = true; // Habilitar rotación automática con el NavMeshAgent
        agent.baseOffset = heightOffset;
        agent.speed = runSpeed;  // Velocidad de correr
        agent.angularSpeed = 500f; // Velocidad de rotación

        rb.isKinematic = true;
        rb.useGravity = true;
    }

    void Update()
    {
        if (playerCamera == null)
        {
            Debug.LogWarning("No se asignó la cámara del jugador al script.");
            return;
        }

        // Calcular la distancia entre el zorro y el jugador
        float distance = Vector3.Distance(transform.position, playerCamera.position);

        if (distance < followDistance)
        {
            // Huir en dirección contraria al jugador
            Vector3 directionAwayFromPlayer = transform.position - playerCamera.position;

            // Verificar si la nueva posición está dentro del NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(transform.position + directionAwayFromPlayer.normalized * runSpeed, out hit, 1f, NavMesh.AllAreas))
            {
                Debug.Log("Destino válido encontrado para huir.");
                agent.isStopped = false;
                agent.SetDestination(hit.position); // Establecer el destino dentro del NavMesh
            }
            else
            {
                Debug.LogWarning("No se encontró un destino válido para huir.");
            }

            animator.SetBool("IsRunning", true);
        }
        else
        {
            // Movimiento aleatorio si no está huyendo
            Vector3 randomDirection = new Vector3(Random.Range(-10, 10), transform.position.y, Random.Range(-10, 10));
            NavMeshHit hitRandom;
            if (NavMesh.SamplePosition(randomDirection, out hitRandom, 1f, NavMesh.AllAreas))
            {
                Debug.Log("Destino aleatorio válido encontrado.");
                agent.isStopped = false;
                agent.SetDestination(hitRandom.position);
            }
            else
            {
                Debug.LogWarning("No se encontró un destino válido para moverse aleatoriamente.");
            }

            animator.SetBool("IsRunning", false);
            animator.SetBool("IsWalking", true);
        }

        // Rotación hacia la dirección del movimiento
        Vector3 direction = (playerCamera.position - transform.position).normalized;
        direction.y = 0;  // Evitar que gire en el eje Y
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        // Asegurarse de que el zorro se mantenga sobre el terreno
        RaycastHit hit2;
        if (Physics.Raycast(transform.position, Vector3.down, out hit2))
        {
            transform.position = new Vector3(transform.position.x, hit2.point.y + heightOffset, transform.position.z);
        }
    }
}
