using UnityEngine;

public abstract class EnemyState 
{
    protected EnemyMovement enemy;

    public EnemyState(EnemyMovement enemyMovement)
    { 
        enemy = enemyMovement; 
    }

    //Método que se llama una vez al entrar en el estado.
    public abstract void EnterState();

    //Método que se llama cada frame mientras el estado está activo.
    public abstract void UpdateState();

    //Método que se llama una vez al salir del estado.
    public abstract void ExitState();
}
