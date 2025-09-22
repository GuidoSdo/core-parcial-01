using UnityEngine;

public class PlayerAttackingSystem : MonoBehaviour
{
    [SerializeField] private float _attackRange; //Rango del Raycast, para cambiarlo mas facilmente si queremos
    [SerializeField] private LayerMask _enemy; //Deteccion del Enemigo

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AttackTry();
        }
    }

        void AttackTry()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, _attackRange, _enemy))
        {
            EnemyBackStab enemigo = hit.transform.GetComponentInChildren<EnemyBackStab>(); //El enemigo debe tener un empty dentro con el script (Para no interferir con el collider)

            if (enemigo != null && enemigo._playerOnBack)
            {
                Debug.Log("Exito en el ataque");
            }
            else
            {
                Debug.Log("Fallo en el Ataque");
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * _attackRange);
    }
}
