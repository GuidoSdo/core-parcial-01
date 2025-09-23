using UnityEngine;

public class ChaseState : EnemyState
{
    public ChaseState(EnemyPatrolAndChase enemyMovement) : base(enemyMovement) { }

    public override void EnterState() { }

    public override void UpdateState()
    {
        // Perseguir al jugador mientras est� a la vista.
        if (enemy.CheckForPlayer())
        {
            enemy.navMeshAgent.SetDestination(enemy.player.position);
            if (Vector3.Distance(enemy.transform.position, enemy.player.position) <= enemy.GetAttackRange())
            {
                Debug.Log("Player is getting attacked");
            }
        }
        // En caso de que el jugador salga del rango de visi�n cambia al estado de alerta
        else
        {
            enemy.ChangeState(new AlertState(enemy));
        }
    }

    public override void ExitState() { }
}