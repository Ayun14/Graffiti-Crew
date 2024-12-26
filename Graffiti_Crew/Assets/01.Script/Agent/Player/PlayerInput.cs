using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action<Vector3> MovementEvent;

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
    }
}
