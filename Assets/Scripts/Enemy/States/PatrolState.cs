using UnityEngine;

/// <summary>
/// Estado de patrulla del enemigo.
/// Se encarga de mover al enemigo entre los puntos de patrulla definidos usando NavMeshAgent.
/// Cambia al estado de persecución si detecta al jugador.
/// </summary>
public class PatrolState : EnemyState
{
    public PatrolState(EnemyPatrolAndChase enemyMovement) : base(enemyMovement) { }

    public override void EnterState()
    {
        // Al entrar en el estado de patrulla, establece el punto de destino 
        if (enemy.GetPatrolPoints().Length > 0)
        {
            enemy.navMeshAgent.SetDestination(enemy.GetPatrolPoints()[enemy.GetCurrentPointIndex()].position);
        }
    }

    public override void UpdateState()
    {
        CheckForPlayerAndChangeState();
        HandlePatrolMovement();
    }

    private void CheckForPlayerAndChangeState()
    {
        if (enemy.CheckForPlayer())
        {
            enemy.ChangeState(new ChaseState(enemy));
        }
    }

    private void HandlePatrolMovement()
    {
        if (enemy.navMeshAgent.remainingDistance < 0.5f && !enemy.navMeshAgent.pathPending)
        {
            if (enemy.GetCurrentPointIndex() >= enemy.GetPatrolPoints().Length - 1)
            {
                enemy.SetPatrolDirection(-1);
            }
            else if (enemy.GetCurrentPointIndex() <= 0)
            {
                enemy.SetPatrolDirection(1);
            }

            enemy.IncrementCurrentPointIndex();
            enemy.navMeshAgent.SetDestination(enemy.GetPatrolPoints()[enemy.GetCurrentPointIndex()].position);
        }
    }

    public override void ExitState() { }
}