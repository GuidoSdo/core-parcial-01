using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    private float playerSpeed = 10f;
    [SerializeField]
    private float crouchMultiplier = 0.5f; // Multiplicador de velocidad al agacharse.

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

    private void InitializePlayerComponents()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        string missing = null;
        if (rb == null) missing = "Rigidbody";
        if (col == null) missing = missing == null ? "Collider" : missing + ", Collider";
        if (GetComponent<PlayerInput>() == null) missing = missing == null ? "PlayerInput" : missing + ", PlayerInput";
        if (missing != null)
        {
            Debug.LogWarning($"[PlayerController] El prefab del jugador no tiene los siguientes componentes requeridos: {missing}. El movimiento y las colisiones no funcionarán correctamente.");
            throw new MissingComponentException($"[PlayerController] Faltan los siguientes componentes en el prefab del jugador: {missing}");
        }
    }

    private void SetCenterOfMass()
    {
        // Ajustar el centro de masa al piso para evitar que el prefab se caiga.
        // https://docs.unity3d.com/ScriptReference/Rigidbody-centerOfMass.html
        Vector3 localBase = col.bounds.center - new Vector3(0, col.bounds.extents.y, 0) - transform.position;
        rb.centerOfMass = localBase;
    }

    // Invocado por PlayerInput: movimiento.
    public void OnMove(InputValue movementValue)
    {
        lastInput = movementValue.Get<Vector2>();
    }

    // Invocado por PlayerInput: agachado.
    public void OnCrouch(InputValue value)
    {
        // TODO: Mejorar lógica de agachado.
        if (value.isPressed)
        {
            isCrouching = !isCrouching;
            Debug.Log($"Estado de agachado: {isCrouching}");
        }
    }

    private void FixedUpdate()
    {
        // ** Sección para manejar el movimiento continuo. **
        // Velocidad efectiva (aplica multiplicador si está agachado).
        float finalSpeed = isCrouching ? playerSpeed * crouchMultiplier : playerSpeed;
        // Física + input continuo.
        // Aplicar movimiento.
        playerMovement.OnMove(lastInput, rb, finalSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Recoger ítems.
        if (other.gameObject.CompareTag("PickUp"))
            other.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected with " + collision.gameObject.name);
        // Morir al tocar enemigos.
        // if (collision.gameObject.CompareTag("Enemy"))
        //   Destroy(gameObject);
    }
}
