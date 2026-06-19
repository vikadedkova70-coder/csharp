using UnityEngine;
using UnityEngine.AI;
using Adventure.Utils;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State _startingState;
    [SerializeField] private float _roaningDistanceMax = 7f;
    [SerializeField] private float _roaningDistanceMin = 3f;
    [SerializeField] private float _roaningTimerMax = 2f;

    [SerializeField] private bool _isChasingEnemy = false;
    private float _chasingDistance = 4f;
    private float _chasingSpeedMultiplayer = 2f;


    private NavMeshAgent _navMeshAgent;
    private State _currentState;
    private float _roaningTimer;
    private Vector3 _roanPosition;
    private Vector3 _startingPosition;

    private float _roaningSpeed;
    private float _chasingSpeed;

    private enum State
    {
        Idle,
        Roaning,
        Chasing,
        Attacking,
        Death
    }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;

        _currentState = _startingState;

        _roaningSpeed = _navMeshAgent.speed;
        _chasingSpeed = _navMeshAgent.speed * _chasingSpeedMultiplayer;
    }

    private void Update()
    {
        StateHandler();
    }

    private void StateHandler()
    {
        switch (_currentState)
        {
            case State.Roaning:
                _roaningTimer -= Time.deltaTime;
                if (_roaningTimer < 0)
                {
                    Roaning();
                    _roaningTimer = _roaningTimerMax;
                }
                break;

            case State.Attacking:
                break;
            case State.Chasing:
                ChasingTarget();
                CheckCurrentState();
                break;
            case State.Death:
                break;
            default:
            case State.Idle:
                break;
        }
    }

    private void ChasingTarget()
    {
        _navMeshAgent.SetDestination(Player.Instance.transform.position);
    }

    private void CheckCurrentState()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);

        State newState = State.Roaning;

        if (_isChasingEnemy)
        {
            if (distanceToPlayer <= _chasingDistance)
            {
                newState = State.Chasing;
            }
        }

        if (newState != _currentState)
        {
            if (newState == State.Chasing)
            {
                _navMeshAgent.ResetPath();
                _navMeshAgent.speed = _chasingSpeed;
            }
            else if (newState == State.Roaning)
            {
                _roaningTimer = 0f;
                _navMeshAgent.speed = _roaningSpeed;
            }

                _currentState = newState;
        }
    }

    public bool IsRunning()
    {
        if (_navMeshAgent.velocity == Vector3.zero)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void Roaning()
    {
        _startingPosition = transform.position;
        _roanPosition = GetRoaningPosition();
        ChangeFacingDirection(_startingPosition, _roanPosition);
        _navMeshAgent.SetDestination(_roanPosition);
    }

    private Vector3 GetRoaningPosition()
    {
        return _startingPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(_roaningDistanceMin, _roaningDistanceMax);
    }

    private void ChangeFacingDirection(Vector3 sourcePosition, Vector3 targetPosition)
    {
        if (sourcePosition.x > targetPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
