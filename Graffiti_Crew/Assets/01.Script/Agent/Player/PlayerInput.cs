using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private LayerMask _whatIsInteration;

    public event Action<Vector3> MovementEvent;
    public event Action<IInterationObject> InteractionEvent;
    public Vector3 MousePosition { get; private set; }

    private bool _playerInputEnabled = true;

    private void Start()
    {
        MovementEvent?.Invoke(transform.position);
    }

    private void Update()
    {
        if (_playerInputEnabled == false) return;

        CheckMoveInput();
    }

    public void SetPlayerInput(bool enabled)
    {
        _playerInputEnabled = enabled;
    }

    private void CheckMoveInput()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                MovementEvent?.Invoke(hit.point);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), 
                out hit, Mathf.Infinity, _whatIsInteration))
            {
            }
        }
    }
}
