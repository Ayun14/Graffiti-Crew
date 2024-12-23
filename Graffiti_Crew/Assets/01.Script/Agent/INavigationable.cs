using UnityEngine;

public interface INavigationable
{
    public void StopImmediately(bool isActive);
    public void SetDestination();
    public bool CanMoveCheck();
}
