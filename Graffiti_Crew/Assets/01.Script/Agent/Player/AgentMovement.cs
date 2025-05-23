using UnityEngine;
using UnityEngine.AI;

public class AgentMovement : MonoBehaviour, INavigationable
{
    protected NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void StopImmediately(bool isActive)
    {
        _navMeshAgent.isStopped = isActive;
    }

    public void SetDestination(Vector3 destination)
    {
        _navMeshAgent.SetDestination(destination);
    }

    public bool CanMoveCheck()
    {
        return !_navMeshAgent.pathPending &&
           _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance;
    }
}
