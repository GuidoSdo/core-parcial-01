using UnityEngine;

public class PatrolState : EnemyState
{
    public PatrolState(EnemyMovement enemyMovement) : base(enemyMovement) { }

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
        // Verifica si el jugador se encuentra en rango de visión
        if (enemy.CheckForPlayer())
        {
            enemy.ChangeState(new ChaseState(enemy));
        }

        // Si el enemigo llegó al punto de destino, se mueve al siguiente punto
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