using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private LayerMask _whatIsInteration;
    [SerializeField] private LayerMask _whatIsGround;

    public event Action<Vector3> MovementEvent;
    public event Action<InteractionObject> InteractionEvent;
    public Vector3 MousePosition { get; private set; }

    private bool _playerInputEnabled = true;

    private void Start()
    {
        MovementEvent?.Invoke(transform.position);
    }

    private void Update()
    {
        if (_playerInputEnabled == false) return;
        //Debug.Log(_playerInputEnabled);

        CheckMoveInput();
    }

    public void SetPlayerInput(bool enabled)
    {
        Debug.Log(enabled);
        _playerInputEnabled = enabled;
    }

    private void CheckMoveInput()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _whatIsInteration))
            {
                InteractionEvent?.Invoke(hit.transform.GetComponent<InteractionObject>());
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, _whatIsGround))
            {
                if (IsPointOnNavMesh(hit.point))
                {
                    MovementEvent?.Invoke(hit.point);
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _whatIsInteration))
            {
                InteractionEvent?.Invoke(hit.transform.GetComponent<InteractionObject>());
            }
        }
    }

    private bool IsPointOnNavMesh(Vector3 point)
    {
        NavMeshHit navMeshHit;
        return NavMesh.SamplePosition(point, out navMeshHit, 0.5f, NavMesh.AllAreas);
    }
}
