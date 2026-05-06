using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float _detectionRange = 20f;
    [SerializeField] private float _stopFollowingDistance = 25f;
    
    private Transform _player;
    private NavMeshAgent _agent;
    private Animator _animator;
    private bool _isFollowing = false;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        if (_animator == null) _animator = GetComponentInChildren<Animator>();
        
        // Find player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) _player = playerObj.transform;

        // Ensure there's a Kinematic Rigidbody for trigger detection
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    private void Update()
    {
        if (_player == null) return;

        float distance = Vector3.Distance(transform.position, _player.position);

        if (!_isFollowing && distance < _detectionRange)
        {
            _isFollowing = true;
            Debug.Log($"{gameObject.name} started following player.");
        }
        else if (_isFollowing && distance > _stopFollowingDistance)
        {
            _isFollowing = false;
            if (_agent.isOnNavMesh) _agent.ResetPath();
            Debug.Log($"{gameObject.name} stopped following player.");
        }

        if (_isFollowing && _agent.isOnNavMesh)
        {
            _agent.SetDestination(_player.position);
        }

        if (_animator != null)
        {
            float speed = _agent.velocity.magnitude / _agent.speed;
            _animator.SetFloat("Speed", speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EliminatePlayer();
        }
    }

    private void EliminatePlayer()
    {
        Debug.Log("Player Touched by Enemy! Restarting level.");
        // Reset current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
