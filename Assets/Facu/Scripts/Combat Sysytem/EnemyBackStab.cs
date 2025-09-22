using UnityEngine;

public class EnemyBackStab : MonoBehaviour
{
    [HideInInspector] public bool _playerOnBack = false; //Habilita al player a atacar


    private void OnTriggerEnter(Collider other) //Un collider en la espalda del enemigo con IsTrigger on
    {
        if (other.CompareTag("Player")) //El player necesita un Tag Player
        {
            _playerOnBack = true;
            Debug.Log("Jugador detectado con Exito");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerOnBack = false;
            Debug.Log("Jugador salio con exito");
        }
    }

}
