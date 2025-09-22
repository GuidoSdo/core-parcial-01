using UnityEngine;
using UnityEngine.AI;

public class EnemyFollowPlayer : MonoBehaviour
{

 // Reference to the player's transform.
    public Transform player;

     // Reference to the NavMeshAgent component for pathfinding.
    private NavMeshAgent navMeshAgent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
           if (player != null)
       {    
           navMeshAgent.SetDestination(player.position);
       } 
    }
}
