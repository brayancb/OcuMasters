using UnityEngine;
using UnityEngine.AI;

public class RandomWalker : MonoBehaviour
{
    public float wanderRadius = 10f;  // Radio del área para pasear
    public float wanderDelay = 3f;   // Tiempo entre cada movimiento
    public float heightOffset = 1f; // Ajuste de altura para evitar que se quede pegado al piso
    public Transform playerCamera;  // Cámara principal del jugador
    public float fleeDistance = 5f; // Distancia mínima para activar la huida
    public float runSpeed = 10f;    // Velocidad al correr
    public float walkSpeed = 3.5f;  // Velocidad al caminar
    public float fleeDuration = 2f; // Duración de la huida

    private NavMeshAgent agent;
    private Animator animator;
    private float timer;
    private bool isFleeing = false;
    private float fleeTimer = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Verificar componentes
        if (agent == null || animator == null)
        {
            Debug.LogError("Faltan componentes NavMeshAgent o Animator.");
            enabled = false;
            return;
        }

        if (playerCamera == null && Camera.main != null)
        {
            playerCamera = Camera.main.transform; // Asignar la cámara principal automáticamente
        }

        agent.baseOffset = heightOffset; // Ajustar la altura inicial del agente
    }

    void Update()
    {
        if (isFleeing)
        {
            HandleFleeing();
        }
        else
        {
            HandleWandering();
        }

        // Asegurarse de que el zorro se mantenga sobre el terreno
        AdjustHeightToGround();

        // Actualizar animaciones según el movimiento
        UpdateAnimations();
    }

    void HandleFleeing()
    {
        fleeTimer += Time.deltaTime;
        if (fleeTimer >= fleeDuration)
        {
            isFleeing = false; // Terminar huida
            agent.speed = walkSpeed; // Restaurar velocidad de caminar
            animator.SetBool("IsRunning", false); // Desactivar animación de correr
            return;
        }

        // Seguir huyendo si el tiempo de huida no ha terminado
        if (!agent.hasPath)
        {
            // Calcular la dirección opuesta al jugador
            Vector3 fleeDirection = (transform.position - playerCamera.position).normalized;
            Vector3 fleeTarget = transform.position + fleeDirection * fleeDistance;

            // Encontrar un punto válido en el NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(fleeTarget, out hit, fleeDistance, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
                Debug.Log("Zorro huyendo a: " + hit.position);
            }
        }
    }

    void HandleWandering()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerCamera.position);

        // Si el jugador está demasiado cerca, activar huida
        if (distanceToPlayer <= fleeDistance)
        {
            StartFleeing();
            return;
        }

        // Caminar aleatoriamente si no está huyendo
        timer += Time.deltaTime;
        if (timer >= wanderDelay)
        {
            Vector3 newPos = GetRandomPoint(transform.position, wanderRadius);
            if (newPos != Vector3.zero)
            {
                agent.SetDestination(newPos);
                Debug.Log("Nuevo destino establecido: " + newPos);
            }
            timer = 0; // Reiniciar el temporizador
        }
    }

    void StartFleeing()
    {
        isFleeing = true;
        fleeTimer = 0f;

        // Activar animación de correr
        animator.SetBool("IsRunning", true);

        // Aumentar la velocidad del agente para correr
        agent.speed = runSpeed;

        // Calcular la dirección opuesta al jugador
        Vector3 fleeDirection = (transform.position - playerCamera.position).normalized;
        Vector3 fleeTarget = transform.position + fleeDirection * fleeDistance;

        // Encontrar un punto válido en el NavMesh
        NavMeshHit hit;
        if (NavMesh.SamplePosition(fleeTarget, out hit, fleeDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    Vector3 GetRandomPoint(Vector3 center, float radius)
    {
        Vector2 randomPoint = Random.insideUnitCircle * radius;
        Vector3 randomPosition = center + new Vector3(randomPoint.x, 0, randomPoint.y);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPosition, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return Vector3.zero;
    }

    void AdjustHeightToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y + heightOffset, transform.position.z);
        }
    }

    void UpdateAnimations()
    {
        if (isFleeing)
        {
            return; // Animaciones ya controladas en el estado de huida
        }

        // Determinar si está caminando
        bool isWalking = agent.velocity.sqrMagnitude > 0.1f;
        animator.SetBool("IsWalking", isWalking);
    }
}
