using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Clase que permite que el enemigo siga al jugador utilizando el sistema NavMeshAgent.
/// Asigna el destino del enemigo a la posición actual del jugador en cada frame.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
public class EnemyFollowPlayer : MonoBehaviour
{

    public Transform player;

    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

    }
    void Update()
    {
        if (player != null)
        {
            navMeshAgent.SetDestination(player.position);
        }
    }
}
