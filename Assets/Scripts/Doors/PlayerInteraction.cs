using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 3f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            DoorManager[] doors = FindObjectsOfType<DoorManager>();

            DoorManager closestDoor = null;
            float closestDist = Mathf.Infinity;

            foreach (DoorManager door in doors)
            {
                float dist = (door.transform.position - transform.position).magnitude;
                if (dist < interactDistance && dist < closestDist)
                {
                    closestDist = dist;
                    closestDoor = door;
                }
            }

            // Si hay una puerta cerca, intentar abrirla
            if (closestDoor != null)
            {
                Inventory inv = GetComponent<Inventory>();
                closestDoor.TryOpenDoor(inv);
            }
        }
    }
}