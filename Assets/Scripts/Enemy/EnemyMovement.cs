using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class EnemyMovement : MonoBehaviour
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
    public bool CheckForPlayer()
    {
        if (player == null)
        {
            Debug.LogWarning("[EnemyMovement] No se puede detectar al jugador porque 'player' es null.");
            return false;
        }
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, sightRange, playerLayer);
        foreach (var hitCollider in hitColliders)
        {
            Vector3 directionToPlayer = (hitCollider.transform.position - transform.position).normalized;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, sightRange, obstacleLayer))
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
