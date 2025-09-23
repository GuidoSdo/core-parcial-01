using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private LayerMask environmentMask;

    [SerializeField] const float RAYCAST_DISTANCE = 10f;

    void Update()
    {
        if (playerMask == 0)
        {
            Debug.LogWarning("[FieldOfView] LayerMask de player no asignado.");
            return;
        }
        Vector3 eyePosition = transform.position;
        eyePosition.y = transform.position.y + 1;

        // Dispara un solo rayo que puede detectar tanto al jugador como a un obstáculo
        if (Physics.Raycast(eyePosition, transform.forward, out RaycastHit hit, RAYCAST_DISTANCE, playerMask | obstacleMask | environmentMask, QueryTriggerInteraction.Ignore))
        {
            Debug.DrawRay(eyePosition, transform.forward * hit.distance, Color.green);

            // Verifica si el objeto golpeado se encuentra en la capa del jugador
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Player"))
            {

                // LLama al método que verifica si hay un objeto entre el jugador y el enemigo
                if (CheckForObstacleBetween(hit.point, eyePosition))
                {
                    Debug.Log("There is an obtacle");
                }
                else
                {
                    Debug.Log("Player Spotted");
                }

            }
            // Verifica si el objeto golpeado se encuentra en la capa de obstáculo o environment
            else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Environment") || hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                Debug.Log("Environment");
            }
        }
        else
        {
            Debug.Log("Nothing");
            Debug.DrawRay(eyePosition, transform.forward * RAYCAST_DISTANCE, Color.red);
        }
    }

    // Método que verifica si hay un obstáculo entre el rayo de origen y el jugador.
    private bool CheckForObstacleBetween(Vector3 playerHitPoint, Vector3 eyeLevel)
    {
        Vector3 direction = playerHitPoint - eyeLevel;
        float distance = direction.magnitude;

        // Lanza un rayo desde la posición del jugador hacia el punto de origen del rayo original 
        if (Physics.Raycast(playerHitPoint, -direction, distance, obstacleMask | environmentMask, QueryTriggerInteraction.Ignore))
        {
            return true;
        }

        return false;
    }
}
