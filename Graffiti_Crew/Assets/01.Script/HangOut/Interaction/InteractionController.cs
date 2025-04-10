using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private Player _player;

    [Header("UI Elements")]
    [SerializeField] private GameObject _interactionCanvas;
    [SerializeField] private RectTransform _canvasRect;
    [SerializeField] private RectTransform _interactionIndicator;
    [SerializeField] private List<InteractionObject> _interactionObjects;

    [Header("Zoom Data")]
    [SerializeField] private CinemachineCamera _cam;
    private float _zoomSpeed = 10f;

    private Camera _mainCamera;
    private InteractionObject _currentTarget;

    private void Start()
    {
        _mainCamera = Camera.main;
        _interactionCanvas.SetActive(false);
        _interactionIndicator.gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateCurrentTarget();
        UpdateUI();

        CheckZoom();
    }

    private void CheckZoom()
    {
        if (_player.StateMachine.CurrentStateEnum == PlayerStateEnum.Sit
            || _player.StateMachine.CurrentStateEnum == PlayerStateEnum.Mirror
            || _player.StateMachine.CurrentStateEnum == PlayerStateEnum.BoomBox)
            Zoom();
        else
            _cam.Lens.FieldOfView = 12;
    }

    private void Zoom()
    {
        float distance = Input.GetAxis("Mouse ScrollWheel") * -1 * _zoomSpeed;
        float camFOV = _cam.Lens.FieldOfView + distance;
        if (distance != 0 && camFOV >= 4 && camFOV <= 12)
        {
            _cam.Lens.FieldOfView += distance;
        }
    }

    private void UpdateCurrentTarget()
    {
        _currentTarget = null;
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        foreach (var interaction in _interactionObjects)
        {
            if (interaction.stateEnum == _player.StateMachine.CurrentStateEnum)
                continue;

            if (CheckMousePos(interaction.Col))
            {
                if (_currentTarget != null)
                    _currentTarget.interactionImg.enabled = false;
                _currentTarget = interaction;
                break;
            }
        }
    }

    private void UpdateUI()
    {
        if (_currentTarget == null)
        {
            _interactionCanvas.SetActive(false);
            _interactionIndicator.gameObject.SetActive(false);
            _currentTarget = null;
            return;
        }

        Vector3 screenPos = _mainCamera.WorldToViewportPoint(_currentTarget.interactionImg.transform.position);
        bool isVisible = screenPos.z > 0 && screenPos.x > 0 && screenPos.x < 1 && screenPos.y > 0 && screenPos.y < 1;

        if (isVisible)
        {
            _interactionCanvas.SetActive(false);
            _interactionIndicator.gameObject.SetActive(false);
            _currentTarget.interactionImg.enabled = true;
            _currentTarget.interactionImg.transform.LookAt(_mainCamera.transform);
        }
        else
        {
            _currentTarget.interactionImg.enabled = false;
            ShowIndicator(_currentTarget);
        }
    }

    private void ShowIndicator(InteractionObject obj)
    {
        Vector3 screenPos = _mainCamera.WorldToScreenPoint(obj.interactionImg.transform.position);
        screenPos.x = Mathf.Clamp(screenPos.x, 50, Screen.width - 50);
        screenPos.y = Mathf.Clamp(screenPos.y, 50, Screen.height - 50);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, screenPos, _mainCamera, out Vector2 localPoint);

        _interactionCanvas.SetActive(true);
        _interactionIndicator.anchoredPosition = localPoint;
        _interactionIndicator.gameObject.SetActive(true);

        Vector3 dir = (obj.TargetPos - _mainCamera.transform.position).normalized;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        //_interactionIndicator.rotation = Quaternion.Euler(0, 0, -angle);
    }

    private bool CheckMousePos(Collider col)
    {
        if (col != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            return col.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity);
        }

        return false;
    }
}
