using UnityEngine;

public class SecurityCameraMovement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float _rotationSpeed; //La velocidad a la que se mueve, determinar cual creemos mas optima
    [SerializeField] private float _maxRotation = 45f;  //Esto tiene que ser 45 en caso de esquinas o 90 en caso de pared recta

    private float _currentAngle = 0f; //Esto tiene que quedar asi, guarda la informacion de cual es el angulo actual, 0 determina la posicion donde nosotros lo dejamos mirando
    private int _direction = 1; //Esto es para que se mueva de derecha a izquierda

    private void Update()
    {
        float rotationStep = _rotationSpeed * Time.deltaTime * _direction;
        transform.Rotate(0, rotationStep, 0);

        _currentAngle += rotationStep;

        if (Mathf.Abs(_currentAngle) >= _maxRotation) //Para que gire al llegar a la rotacion deseada
        {
            _direction *= -1;
        }
    }
}
