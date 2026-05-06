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
        // Ensure center is correct
        _controller.center = new Vector3(0, 1f, 0);
        _controller.height = 2f;
        _controller.radius = 0.5f;

        _animator = GetComponent<Animator>();
        if (_animator == null) _animator = GetComponentInChildren<Animator>();
        
        if (_animator != null)
        {
            _animator.applyRootMotion = false;
        }

        if (Camera.main != null)
        {
            _mainCamera = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning("No MainCamera found. Player movement may not be relative to camera.");
        }

        if (_inputActions != null)
        {
            var playerActionMap = _inputActions.FindActionMap("Player");
            if (playerActionMap != null)
            {
                _moveAction = playerActionMap.FindAction("Move");
            }
            else
            {
                Debug.LogError("Player Action Map not found in InputActions asset.");
            }
        }
        else
        {
            Debug.LogError("InputActions asset not assigned to PlayerController.");
        }
    }

    private void OnEnable()
    {
        if (_moveAction != null) _moveAction.Enable();
    }

    private void OnDisable()
    {
        if (_moveAction != null) _moveAction.Disable();
    }

    private void Update()
    {
        if (_moveAction != null)
        {
            _moveInput = _moveAction.ReadValue<Vector2>();
        }
        
        Move();
        ApplyGravity();

        if (_animator != null)
        {
            float speed = _moveInput.magnitude;
            _animator.SetFloat("Speed", speed);
        }
    }

    private void Move()
    {
        if (_moveInput.magnitude < 0.1f) return;

        Vector3 moveDirection;
        if (_mainCamera != null)
        {
            // Calculate direction relative to camera
            Vector3 forward = _mainCamera.forward;
            Vector3 right = _mainCamera.right;
            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();
            moveDirection = forward * _moveInput.y + right * _moveInput.x;
        }
        else
        {
            moveDirection = new Vector3(_moveInput.x, 0, _moveInput.y);
        }

        if (moveDirection != Vector3.zero)
        {
            // Rotate character
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);

            // Move character
            _controller.Move(moveDirection * _moveSpeed * Time.deltaTime);
        }
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
