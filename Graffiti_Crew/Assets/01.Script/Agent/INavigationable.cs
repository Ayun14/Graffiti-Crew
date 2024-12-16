using UnityEngine;

public interface INavigationable
{
    public void StopImmediately();
    public void SetDestination(Vector3 destination);
    public bool CheckDistance();
}
