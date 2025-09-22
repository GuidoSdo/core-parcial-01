using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour
{
    // --- Atributos de Configuración ---
    [Header("Enemy Patrol Settings")]
    [SerializeField] private Transform[] enemyPatrolPoints; //Vincular los puntos de patrulla
    
    

    // --- Atributos de detección ---
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
    public int GetCurrentPointIndex() { return currentEnemyPointIndex;}
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

    //Método que cambia el estado del enemigo
    public void ChangeState(EnemyState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }
        currentState = newState;
        currentState.EnterState();
    }

    //Método que verifica si el jugador está a la vista
    public bool CheckForPlayer()
    {
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
        
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Establece el punto de origen del patrón de patrulla
        if (enemyPatrolPoints.Length > 0)
        {
            currentEnemyPointIndex = 0;
            navMeshAgent.enabled = false;
            transform.position = enemyPatrolPoints[currentEnemyPointIndex].position;
            navMeshAgent.enabled = true;

            ChangeState(new PatrolState(this));
        }
    }

    void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState();
        }
    }

   
}
