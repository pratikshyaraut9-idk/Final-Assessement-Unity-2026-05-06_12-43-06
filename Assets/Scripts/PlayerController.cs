using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private float _gravity = -9.81f;

    [Header("Input Actions")]
    [SerializeField] private InputActionAsset _inputActions;
    
    private CharacterController _controller;
    private Animator _animator;
    private InputAction _moveAction;
    private Vector2 _moveInput;
    private Vector3 _velocity;
    private Transform _mainCamera;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        if (_animator == null) _animator = GetComponentInChildren<Animator>();
        _mainCamera = Camera.main.transform;

        var playerActionMap = _inputActions.FindActionMap("Player");
        _moveAction = playerActionMap.FindAction("Move");
    }

    private void OnEnable()
    {
        _moveAction.Enable();
    }

    private void OnDisable()
    {
        _moveAction.Disable();
    }

    private void Update()
    {
        _moveInput = _moveAction.ReadValue<Vector2>();
        Move();
        ApplyGravity();

        if (_animator != null)
        {
            _animator.SetFloat("Speed", _moveInput.magnitude);
        }
    }

    private void Move()
    {
        if (_moveInput.magnitude < 0.1f) return;

        // Calculate direction relative to camera
        Vector3 forward = _mainCamera.forward;
        Vector3 right = _mainCamera.right;
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = forward * _moveInput.y + right * _moveInput.x;

        // Rotate character
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

        // Move character
        _controller.Move(moveDirection * _moveSpeed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (_controller.isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }
}
