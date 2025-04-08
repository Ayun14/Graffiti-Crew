using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InteractionObject : MonoBehaviour
{
    public PlayerStateEnum stateEnum;
    public Image interactionImg;

    public Vector3 TargetPos => _targetPos;
    private Vector3 _targetPos;

    public Collider Col => _col;
    private Collider _col;

    protected virtual void Awake()
    {
        _col = GetComponent<Collider>();
        _targetPos = transform.Find("Target").position;
    }

    private void Start()
    {
        interactionImg.enabled = false;
    }

}
