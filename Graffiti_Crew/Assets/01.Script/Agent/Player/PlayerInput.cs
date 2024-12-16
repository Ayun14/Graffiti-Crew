using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public event Action<Vector3> MovementEvent;

    public Vector3 MousePosition { get; private set; }

    private bool _playerInputEnabled = true;

    public void SetPlayerInput(bool enabled)
    {
        _playerInputEnabled = enabled;
    }

    private void Update()
    {
        if (_playerInputEnabled == false) return;

        CheckMoveInput();
    }

    private void CheckMoveInput()
    {
        if(Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            {
                MovementEvent?.Invoke(hit.point);
            }
        }
    }
}
