using UnityEngine;

using UnityEngine.InputSystem;

/// <summary>
/// Implementación de movimiento físico para el jugador usando Rigidbody.
/// Referencia: https://docs.unity3d.com/6000.2/Documentation/ScriptReference/Rigidbody-linearVelocity.html
/// </summary>
public class PhysicsPlayerMovement : IPlayerMovement
{
    public void OnMove(Vector2 movementInput, Rigidbody rb, float speed)
    {
        Vector3 move = new(movementInput.x, 0f, movementInput.y);

        // Normalizamos para evitar que en diagonal vaya más rápido
        if (move.sqrMagnitude > 1f)
        {
            move.Normalize();
        }

        float currentY = rb.linearVelocity.y;

        rb.linearVelocity = new Vector3(
            move.x * speed,
            currentY,
            move.z * speed
        );
    }
}
