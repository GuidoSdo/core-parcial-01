using UnityEngine;

public interface IPlayerMovement
{
    void OnMove(Vector2 movementInput, Rigidbody rb, float speed);
}
