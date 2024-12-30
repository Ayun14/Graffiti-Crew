using System;
using UnityEngine;

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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _whatIsInteration))
            {
                InteractionEvent?.Invoke(hit.transform.GetComponent<InteractionObject>());
            }
            else if(Physics.Raycast(ray, out hit, Mathf.Infinity, _whatIsGround))
            {
                MovementEvent?.Invoke(hit.point);
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _whatIsInteration))
            {
                InteractionEvent?.Invoke(hit.transform.GetComponent<InteractionObject>());
            }
        }
    }
}
