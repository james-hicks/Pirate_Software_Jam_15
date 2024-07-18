using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _turnSpeed = 360f;
    [SerializeField] private Transform _playerModel;

    private Vector3 moveInput;

    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Look();
    }


    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        _rb.MovePosition(transform.position + moveInput.ToIso() * moveInput.normalized.magnitude * _speed * Time.deltaTime);
    }
    private void Look()
    {
        if (moveInput == Vector3.zero) return;

        Quaternion rot = Quaternion.LookRotation(moveInput.ToIso(), Vector3.up);
        _playerModel.rotation = Quaternion.RotateTowards(_playerModel.rotation, rot, _turnSpeed * Time.deltaTime);
    }

    #region Input
    private void OnMove(InputValue value)
    {
        Vector2 Input = value.Get<Vector2>();
        moveInput = new Vector3(Input.x, 0, Input.y);
    }
     
    private void OnThrow(InputValue value)
    {

    }

    private void OnInteract(InputValue value)
    {

    }
    #endregion
}

public static class Helpers
{
    private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
    public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
}
