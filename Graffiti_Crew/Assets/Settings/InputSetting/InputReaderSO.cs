using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputReaderSO", menuName = "SO/InputReaderSO")]
public class InputReaderSO : ScriptableObject, InputActions.IUIActions {
    public event Action OnCancleEvent;

    private InputActions _inputAction;
    private void Awake() {
    }
    private void OnEnable() {
        if (_inputAction == null) {
            _inputAction = new InputActions();
            _inputAction.UI.SetCallbacks(this);
        }
        _inputAction.UI.Enable();
    }
    private void OnDisable() {
        _inputAction.UI.Disable();
    }
    public void OnCancel(InputAction.CallbackContext context) { // esc누르면 호출
        if (context.performed) {
            OnCancleEvent?.Invoke();
        }
    }
    #region Hide
    public void OnClick(InputAction.CallbackContext context) {

    }
    public void OnMiddleClick(InputAction.CallbackContext context) {

    }
    public void OnNavigate(InputAction.CallbackContext context) {

    }
    public void OnPoint(InputAction.CallbackContext context) {

    }
    public void OnRightClick(InputAction.CallbackContext context) {

    }
    public void OnScrollWheel(InputAction.CallbackContext context) {

    }
    public void OnSubmit(InputAction.CallbackContext context) {

    }
    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context) {

    }
    public void OnTrackedDevicePosition(InputAction.CallbackContext context) {

    }
    #endregion
}
