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

    public void SetDestination()
    {
        _navMeshAgent.SetDestination(_navMeshAgent.destination);
    }

    public bool CanMoveCheck()
    {
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            if (!_navMeshAgent.hasPath || _navMeshAgent.velocity.sqrMagnitude == 0f)
            {
                return false;
            }
        }
        
        return true;
    }
}
