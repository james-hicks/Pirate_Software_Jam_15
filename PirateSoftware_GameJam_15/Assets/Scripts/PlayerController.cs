using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _turnSpeed = 360f;
    [SerializeField] private float _jumpForce = 4;
    [SerializeField] private float _gravityScale = -15f;
    [SerializeField] private Transform _playerModel;

    [Header("Potion Throwing")]
    [SerializeField] private float _throwForce;
    [SerializeField] private Transform _throwPoint;
    public GameObject _potionPrefab;
    [SerializeField] private float _throwCooldown = 2f;
    private float _lastThrowTime = -Mathf.Infinity;

    public bool HasPotion = false;

    private Vector3 moveInput;

    private Rigidbody _rb;
    private bool _isGrounded => CheckGrounded();
    private Animator _animator;

    public bool HasControl = true;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!HasControl) return;
        Look();
    }


    private void FixedUpdate()
    {
        if (!HasControl) return;
        Move();

        if (!_isGrounded)
        {
            Vector3 gravity = Vector3.up * _gravityScale;
            _rb.AddForce(gravity, ForceMode.Acceleration);
        }
    }

    private void Move()
    {
        _rb.MovePosition(transform.position + moveInput.ToIso() * moveInput.normalized.magnitude * _speed * Time.deltaTime);
        _animator.SetBool("Run", moveInput != Vector3.zero);
    }
    private void Look()
    {
        if (moveInput == Vector3.zero) return;

        Quaternion rot = Quaternion.LookRotation(moveInput.ToIso(), Vector3.up);
        _playerModel.rotation = Quaternion.RotateTowards(_playerModel.rotation, rot, _turnSpeed * Time.deltaTime);
    }

    public void Throw()
    {
        GameObject thrownObject = Instantiate(_potionPrefab, _throwPoint.position, Quaternion.identity);
        thrownObject.GetComponent<Rigidbody>().AddForce(_throwPoint.forward * _throwForce, ForceMode.Impulse);
    }

    public void Jump()
    {
        Vector3 jumpVelocity = Vector3.zero;
        jumpVelocity.y = _jumpForce;
        _rb.AddForce(jumpVelocity, ForceMode.Impulse);
        _animator.SetTrigger("Jump");
    }

    private bool CheckGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 0.1f, transform.position.z), -transform.up, out hit, 0.3f))
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public void PickupPotion()
    {
        HasPotion = true;
        _animator.SetTrigger("Pickup");
    }
        #region Input
    private void OnMove(InputValue value)
    {
        Vector2 Input = value.Get<Vector2>();
        moveInput = new Vector3(Input.x, 0, Input.y);
    }
     
    private void OnThrow(InputValue value)
    {
        if (!HasPotion) return;
        if (Time.time - _lastThrowTime > _throwCooldown)
        {
            _lastThrowTime = Time.time;
            _animator.SetTrigger("Throw");
            //Throw();
        }

    }

    private void OnInteract(InputValue value)
    {
        Debug.LogWarning("Interact not implemented yet");
    }

    private void OnJump(InputValue value)
    {
        if (_isGrounded)
        {
            Jump();
        }

    }

    private void OnPause(InputValue value)
    {
        if (MenuManager.instance.GameIsPaused)
        {
            MenuManager.instance.ResumeGame();
        }
        else
        {
            MenuManager.instance.PauseGame();
        }
    }
    #endregion
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}
