using Unity.VisualScripting;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    
    }
    
    public int getCurrentHealth { get { return currentHealth; } }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(50);
            Debug.Log("Player damaged. Health remaining: " + currentHealth);
        }
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            GameCheckpointManager.Instance.PlayerLostLife();
            currentHealth = maxHealth;

        }
    
    }
}
