using UnityEngine;

public class Pickup : MonoBehaviour
{
    public string keyName;

    void Start()
    {
        keyName = gameObject.name;
    }

    private void OnTriggerEnter(Collider Object)
    {
        Debug.Log(Object);
        Inventory inventory = Object.GetComponentInParent<Inventory>();
        if (inventory != null)
        {
            inventory.AddKey(keyName);
            Destroy(gameObject);
        }
    }
}