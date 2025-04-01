using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InteractionObject : MonoBehaviour
{
    [SerializeField] private Image _interactionImg;

    public PlayerStateEnum stateEnum;
    [HideInInspector] public PlayerStateEnum playerState;

    public Vector3 TargetPos => _targetPos;
    private Vector3 _targetPos;

    private Collider _col;

    protected virtual void Awake()
    {
        _col = GetComponent<Collider>();
        _targetPos = transform.Find("Target").position;
    }

    private void Start()
    {
        _interactionImg.enabled = false;
    }

    private void Update()
    {
        if (CheckMousePos() && playerState != PlayerStateEnum.Interaction)
        {
            if(stateEnum != playerState)
            {
                _interactionImg.transform.LookAt(Camera.main.transform);
                _interactionImg.enabled = true;
            }
        }
        else
        {
            _interactionImg.enabled = false;
        }
    }

    private bool CheckMousePos()
    {
        if (_col != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return _col.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity);
        }

        return false;
    }
}
