using UnityEngine;

public class AlertState : EnemyState
{
    private float alertTimer;

    public AlertState(EnemyMovement enemyMovement) : base(enemyMovement) { }

    public override void EnterState()
    {
        alertTimer = enemy.GetAlertDuration();
    }

    public override void UpdateState()
    {
        if (enemy.CheckForPlayer())
        {
            enemy.ChangeState(new ChaseState(enemy));
            return;
        }

        // Cuenta regresiva para volver al estado de Patrulla una vez que dejó de ver al player
        if (enemy.navMeshAgent.remainingDistance < 0.5f)
        {
            alertTimer -= Time.deltaTime;
        }

        if (alertTimer <= 0)
        {
            enemy.ChangeState(new PatrolState(enemy));
        }
    }

    public override void ExitState() { }
}