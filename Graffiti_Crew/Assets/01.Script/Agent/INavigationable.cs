using UnityEngine;

public interface INavigationable
{
    public void StopImmediately(bool isActive);
    public void SetDestination(Vector3 destination);
    public bool CanMoveCheck();
}
