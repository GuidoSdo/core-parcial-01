using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    
    [SerializeField] private int checkpointID;
    private GameCheckpointManager gameManager;
    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameCheckpointManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameCheckpointManager not found in the scene! Please add a GameCheckpointManager script to a GameObject.");
        }
    }

   
    public int GetCheckpointID()
    { 
        return checkpointID;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.SetCurrentCheckpoint(this);
            Debug.Log("Player reached checkpoint " + this.checkpointID);
        }
    }

}
