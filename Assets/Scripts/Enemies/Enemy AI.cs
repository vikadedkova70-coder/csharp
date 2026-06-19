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
        Roaning
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
            case State.Roaning:
                roaningTime -= Time.deltaTime;
                if (roaningTime < 0)
                {
                    Roaning();
                    roaningTime = roaningTimerMax;
                }
                break;
        }

    }

    private void Roaning()
    {
        startingPosition = transform.position;
        roanPosition = GetRoaningPosition();
        ChangeFacingDirection(startingPosition, roanPosition);
        navMeshAgent.SetDestination(roanPosition);
    }

    private Vector3 GetRoaningPosition()
    {
        return startingPosition + Utils.GetRandomDir() * UnityEngine.Random.Range(roaningDistanceMin, roaningDistanceMax);
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
