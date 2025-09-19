using UnityEngine;

public class GameCheckpointManager : MonoBehaviour
{
  public static GameCheckpointManager Instance;

    [Header("Player Settings")]
    [SerializeField] private Transform player; //Agregar GameObject Player en el inspector
    [SerializeField] private int maxLives = 3;
    private int currentLives;

    [Header("CheckPoint Settings")]
    [SerializeField] private Checkpoint initialCheckpoint; //Agregar GameObject del checkpoint inicial en el inspector
    [SerializeField] private Checkpoint currentCheckpoint;
    private int lastCheckpointID = -1;

    [SerializeField] GameObject playerObject;
    PlayerHealth playerHealth;

    private void Start()
    {
        playerHealth = playerObject.GetComponent<PlayerHealth>();
    }
    private void Awake()
    {
        lastCheckpointID = initialCheckpoint.GetCheckpointID();

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        currentLives = maxLives;

        if (initialCheckpoint != null)
        {
            SetCurrentCheckpoint(initialCheckpoint);
        }
    }

    private void Update()
    {
        if (currentLives == 0 && playerHealth.getCurrentHealth <= 0)
        {
            GameOver();
        }
    }

    // Este m�todo es llamdo por los checkpoints cuando el jugador pasa por ellos.
    public void SetCurrentCheckpoint(Checkpoint newCheckpoint)
    {
        //Solo actualiza el checkpoint si el nuevo ID es mayor que el �ltimo checkpoint que se guard�.
        if (newCheckpoint.GetCheckpointID() > lastCheckpointID)
        {
            currentCheckpoint = newCheckpoint;
            lastCheckpointID = newCheckpoint.GetCheckpointID();
            Debug.Log("Nuevo checkpoint ID: " + lastCheckpointID);
        }
        else 
        {
            Debug.Log("Player return back. Respawn checkpoint is set at " + lastCheckpointID);        
        }
            
    }
   
    // Se llama a este m�todo cuando el jugador pierde una vida
    public void PlayerLostLife()
    {
        currentLives--;
        Debug.Log("Player lost a life. Remaining lives: " + currentLives);

        if (currentLives <= 0)
        {
            GameOver();

        }
        else 
        {
            RespawnAtLastCheckpoint(currentCheckpoint);
        }

    }

   // M�todo que define el �ltimo checkpoint alcanzado por el jugador
    private void RespawnAtLastCheckpoint(Checkpoint currentCheckpoint)
    {
        if (player != null && currentCheckpoint != null)
        {
            player.position = currentCheckpoint.transform.position;
            player.rotation = currentCheckpoint.transform.rotation;

            Debug.Log("Respawning at the last Checkpoint: " + currentCheckpoint.GetCheckpointID());

        }
        else
        {
            Debug.LogError("Player or current checkpoint not assigned!");
        } 
    }

    // M�todo que define si se perdi� el juego al llegar las vidas a 0 
    private void GameOver()
    {
        Debug.Log("Game Over");

        currentLives = maxLives;

        if(player != null && initialCheckpoint != null) 
        { 
            player.position = initialCheckpoint.transform.position;
            player.rotation = initialCheckpoint.transform.rotation;

            //Al activarse el Game Over se resetea el �ltimo checkpoint volviendolo a establecer como checkpoint inicial.
            lastCheckpointID = initialCheckpoint.GetCheckpointID();

            Debug.Log("Respawning at initial checkpoint: " + initialCheckpoint.GetCheckpointID());
        }
        else 
        {
            Debug.LogError("Player or initial checkpoint not assigned!");            
        }

        // Ac� se puede agregar llamdo a la pantalla de derrota en caso de estar en el nivel 1 o devolverlo al checkpoint inicial en caso de tutorial.

    }
}

