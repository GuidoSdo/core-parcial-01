using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] Transform _target;
    [SerializeField] float _smoothSpeed = 100f;

    void LateUpdate()
    {
        if (_target == null) return;
        Vector3 behind = -_target.forward * 2.5f;
        Vector3 desiredPosition = _target.position + behind;

        transform.position = desiredPosition;

        transform.LookAt(_target.position);
    }
}
