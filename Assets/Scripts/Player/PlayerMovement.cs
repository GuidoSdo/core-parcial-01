using System.ComponentModel;
using Unity.Mathematics;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float RunSpeed = 7f;
    [SerializeField] float CrouchSpeed = 3f;
    [SerializeField] float _rotationSpeed = 50f;
    float _speed = 7f;
    Rigidbody rb;
    float _moveH, _moveV;
    Vector3 _movement;
    Vector3 _moveDirection;
    Vector3 _moveSideways;
    private bool stealth = false;
    float animspeed = 1.9f;
    float _rotationAmount;
    Quaternion _turnOffset; bool _jumping = false;

    private Animator _animator;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        _animator = GetComponent<Animator>();
        Debug.Log(_animator);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _jumping = false;
        }
    }

    private void OnFootstep(AnimationEvent animationEvent)
    {

    }

    void FixedUpdate()
    {
        _moveH = Input.GetAxis("Horizontal");
        _moveV = Input.GetAxis("Vertical");
        _moveDirection = transform.forward * _moveV * _speed * Time.deltaTime;

        _moveSideways = Vector3.zero; _rotationAmount = _moveH * _rotationSpeed * Time.deltaTime; _turnOffset = Quaternion.Euler(0, _rotationAmount, 0); rb.MoveRotation(rb.rotation * _turnOffset);

        if (rb.position.y < -25)
        {
            rb.position = new Vector3(-11, 3, 0);
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            stealth = true;
            _speed = CrouchSpeed;
            animspeed = 1.3f;
        }
        else
        {
            _speed = RunSpeed;
            animspeed = 4f;
            stealth = false;
        }
        _movement = rb.position + _moveDirection + _moveSideways;
        rb.MovePosition(_movement);

        if (Mathf.Abs(_moveV) > 0.01f)
        {
            _animator.SetFloat("Speed", Mathf.Abs(_moveV) * animspeed, 0.1f, Time.fixedDeltaTime);
        }
        else
        {
            _animator.SetFloat("Speed", 0f, 0.1f, Time.fixedDeltaTime);
        }

    }

}