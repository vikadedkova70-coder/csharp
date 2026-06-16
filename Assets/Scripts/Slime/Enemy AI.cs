using UnityEngine;
using UnityEngine.AI;
using Adventure.Utils;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State startingState;
    [SerializeField] private float roaningDistanceMax = 7f;
    [SerializeField] private float roaningDistanceMin = 3f;
    [SerializeField] private float roaningTimerMax = 2f;

    private NavMeshAgent navMeshAgent;
    private State state;
    private float roaningTime;
    private Vector3 roanPosition;
    private Vector3 startingPosition;

    private enum State
    {
        Idle,
        Roaning
    }

    private void Start()
    {
        startingPosition = transform.position;
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        state = startingState;
    }

    private void Update()
    {
        switch (state)
        {
            default:
            case State.Idle:
                break;
            case State.Roaning:
                roaningTime -= Time.deltaTime;
                if (roaningTime <= 0)
                {
                    Roaning();
                    roaningTime = roaningTimerMax;
                }
                break;
        }

    }

    private void Roaning()
    {
        roanPosition = GetRoaningPosition();
        navMeshAgent.SetDestination(roanPosition);
    }

    private Vector3 GetRoaningPosition()
    {
        return startingPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(roaningDistanceMin, roaningDistanceMax);
    }
}
