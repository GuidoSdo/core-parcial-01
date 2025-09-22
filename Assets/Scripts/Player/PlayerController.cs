using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{

    public float playerSpeed = 10f;
    public float crouchMultiplier = 0.5f;
    private bool isCrouching = false;

    private Rigidbody rb;
    private Collider col;
    public IPlayerMovement playerMovement;
    private Vector2 lastInput = Vector2.zero;
    void Awake()
    {
        playerMovement ??= new PhysicsPlayerMovement();
        InitializePlayerComponents();
        SetCenterOfMass();
    }

    private void SetCenterOfMass()
    {
        // Ajustar el centro de masa al piso para evitar que el prefab se caiga
        // Documentación oficial: https://docs.unity3d.com/ScriptReference/Rigidbody-centerOfMass.html
        Vector3 localBase = col.bounds.center - new Vector3(0, col.bounds.extents.y, 0) - transform.position;
        rb.centerOfMass = localBase;
    }

    private void InitializePlayerComponents()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        string missing = null;
        if (rb == null)
            missing = "Rigidbody";
        if (col == null)
            missing = missing == null ? "Collider" : missing + ", Collider";
        if (GetComponent<PlayerInput>() == null)
            missing = missing == null ? "PlayerInput" : missing + ", PlayerInput";
        if (missing != null)
        {
            Debug.LogWarning($"[PlayerController] El prefab del jugador no tiene los siguientes componentes requeridos: {missing}. El movimiento y las colisiones no funcionarán correctamente.");
            throw new MissingComponentException($"[PlayerController] Faltan los siguientes componentes en el prefab del jugador: {missing}");
        }
    }

    //  WIP
    void OnTriggerEnter(Collider other)
    {
        // Deactivate the collided object (making it disappear).
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
        }
    }

    // Este método lo invoca automáticamente PlayerInput cuando detecta la acción de movimiento
    public void OnMove(InputValue movementValue)
    {
        lastInput = movementValue.Get<Vector2>();
    }
    private void FixedUpdate()
    {
        // Ajustamos la velocidad si el jugador está agachado
        float finalSpeed = playerSpeed;
        if (isCrouching)
        {
            finalSpeed *= crouchMultiplier;
        }

        // Aplicamos la física linearVelocity y mantenemos el movimiento mientras haya input
        playerMovement.OnMove(lastInput, rb, finalSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }

    // TODO: Mejorar la lógica de agachado. Actualmente el estado se alterna con la misma tecla; se requiere que el jugador permanezca agachado solo mientras la tecla esté presionada.
    public void OnCrouch(InputValue value)
    {
        if (value.isPressed)
        {
            isCrouching = !isCrouching;
            Debug.Log($"Estado de agachado: {isCrouching}");
        }
    }

}
