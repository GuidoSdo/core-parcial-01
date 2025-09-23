using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    [SerializeField] private int checkpointID;
    private GameCheckpointManager gameManager;
    private void Awake()
    {
        gameManager = FindFirstObjectByType<GameCheckpointManager>();
        if (gameManager != null)
        {
            gameManager.SetCurrentCheckpoint(this);
            Debug.Log("Player reached checkpoint " + this.checkpointID);
        }
        else
        {
            Debug.LogError("No GameCheckpointManager found when reaching checkpoint!");
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
