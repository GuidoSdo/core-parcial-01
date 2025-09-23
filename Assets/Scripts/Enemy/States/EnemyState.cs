using UnityEngine;

public abstract class EnemyState 
{
    protected EnemyPatrolAndChase enemy;

    public EnemyState(EnemyPatrolAndChase enemyMovement)
    { 
        enemy = enemyMovement; 
    }

    //M�todo que se llama una vez al entrar en el estado.
    public abstract void EnterState();

    //M�todo que se llama cada frame mientras el estado est� activo.
    public abstract void UpdateState();

    //M�todo que se llama una vez al salir del estado.
    public abstract void ExitState();
}
