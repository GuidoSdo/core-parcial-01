using UnityEngine;

public class VisionConeCamera : MonoBehaviour
{
    [SerializeField] private LayerMask _targetMask;   
    [SerializeField] private LayerMask _obstacleMask; 

    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & _targetMask) != 0)
        {
            Vector3 directionToTarget = (other.transform.position - transform.position).normalized;

            float distance = Vector3.Distance(transform.position, other.transform.position);

            if (!Physics.Raycast(transform.position, directionToTarget, distance, _obstacleMask))
            {
                Debug.Log("Player Detected" + other.name);
            }
            else
            {
                Debug.Log("There is an obstacle between Player and Camera");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & _targetMask) != 0)
        {
            Debug.Log("Player out of Vision");
        }
    }
}
