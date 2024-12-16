using UnityEngine;
using UnityEngine.AI;

public class AgentMovement : MonoBehaviour, INavigationable
{
    protected NavMeshAgent _navMeshAgent;

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    public void StopImmediately()
    {
        _navMeshAgent.isStopped = true;
    }

    public void SetDestination(Vector3 destination)
    {
        _navMeshAgent.SetDestination(destination);
    }

    public bool CheckDistance()
    {
        if (Vector3.Distance(transform.position, _navMeshAgent.destination) >= _navMeshAgent.stoppingDistance)
            return false;
        else
            return true;
    }
}
