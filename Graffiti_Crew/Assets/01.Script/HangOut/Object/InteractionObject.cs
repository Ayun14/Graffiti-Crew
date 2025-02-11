using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    public PlayerStateEnum stateEnum;

    public Vector3 TargetPos => _targetPos;
    private Vector3 _targetPos;

    protected virtual void Awake()
    {
        _targetPos = transform.Find("Target").position;
    }
}
