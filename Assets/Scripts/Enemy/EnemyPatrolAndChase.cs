using UnityEngine;
using System.Collections;

/// <summary>
/// Controlador de IA para enemigos que patrullan entre puntos y persiguen al jugador si lo detectan.
/// Gestiona los estados de patrulla, persecución y alerta usando NavMeshAgent y detección por física.
/// </summary>
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class EnemyPatrolAndChase : MonoBehaviour
{
    // --- Atributos de Configuraci�n ---
    [Header("Enemy Patrol Settings")]
    [SerializeField] private Transform[] enemyPatrolPoints; //Vincular los puntos de patrulla

    // --- Atributos de detecci�n ---
    [Header("Player Detection Settings")]
    [SerializeField] private float sightRange = 10f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float alertDuration = 5f;
    [SerializeField] private LayerMask playerLayer; //Vincular la capa del jugador
    [SerializeField] private LayerMask obstacleLayer; // Vincular la capa de obstaculo

    // --- Atributos de Estado (privados) ---
    [HideInInspector] public UnityEngine.AI.NavMeshAgent navMeshAgent;
    [HideInInspector] public Transform player;
    private EnemyState currentState;
    private int currentEnemyPointIndex = 0;
    private int patrolDirection = 1;

    // Getters y setters
    public Transform[] GetPatrolPoints() { return enemyPatrolPoints; }
    public int GetCurrentPointIndex() { return currentEnemyPointIndex; }
    public float GetSightRange() { return sightRange; }
    public float GetAttackRange() { return attackRange; }
    public float GetAlertDuration() { return alertDuration; }

    public void SetPatrolDirection(int direction)
    {
        patrolDirection = direction;
    }

    public void IncrementCurrentPointIndex()
    {
        currentEnemyPointIndex += patrolDirection;
    }

    //M�todo que cambia el estado del enemigo
    public void ChangeState(EnemyState newState)
    {
        currentState?.ExitState();
        currentState = newState;
        currentState.EnterState();
    }

    //M�todo que verifica si el jugador est� a la vista
    // Usar un buffer estático para evitar asignaciones
    private static readonly Collider[] playerDetectionBuffer = new Collider[8];
    public bool CheckForPlayer()
    {
        if (playerLayer == 0)
        {
            Debug.LogWarning("[EnemyPatrolAndChase] LayerMask de player no asignado.");
            return false;
        }
        if (player == null)
        {
            Debug.LogWarning("[EnemyPatrolAndChase] No se puede detectar al jugador porque 'player' es null.");
            return false;
        }
        int count = Physics.OverlapSphereNonAlloc(transform.position, sightRange, playerDetectionBuffer, playerLayer);
        for (int i = 0; i < count; i++)
        {
            var hitCollider = playerDetectionBuffer[i];
            Vector3 directionToPlayer = (hitCollider.transform.position - transform.position).normalized;
            if (Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, sightRange, obstacleLayer))
            {
                if (hit.transform != hitCollider.transform)
                {
                    continue;
                }
            }
            return true;
        }
        return false;
    }

    // Gizmos para visualizar el rango de detección en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
    void Start()
    {
        InitializeReferences();
        InitializePatrol();
    }

    private void InitializePatrol()
    {
        if (HasPatrolPoints())
        {
            currentEnemyPointIndex = 0;
            // Ahora se usa Warp para mover el agente correctamente en el NavMesh:
            // Ver documentación oficial: https://docs.unity3d.com/ScriptReference/AI.NavMeshAgent.Warp.html
            navMeshAgent.Warp(enemyPatrolPoints[currentEnemyPointIndex].position);

            ChangeState(new PatrolState(this));
        }
        else
        {
            Debug.LogWarning("[EnemyMovement] No se establecieron puntos de patrulla en 'enemyPatrolPoints'. El enemigo no podrá patrullar.");
        }
    }

    // Verifica si hay puntos de patrulla asignados
    private bool HasPatrolPoints()
    {
        return enemyPatrolPoints.Length > 0;
    }


    private void InitializeReferences()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        else
        {
            Debug.LogWarning("[EnemyMovement] No se encontró un GameObject con el tag 'Player'. El patrullaje funcionará, pero no la detección/persecución.");
            player = null;
        }
    }

    void Update()
    {
        currentState?.UpdateState();
    }
}
