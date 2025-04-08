using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionController : MonoBehaviour
{
    [SerializeField] private Player _player;

    [SerializeField] private RectTransform _canvasRect;
    [SerializeField] private RectTransform _interactionIndicator;

    private Camera _mainCamera;
    private InteractionObject _currentTarget;
    private List<InteractionObject> _interactionObjects = new();

    private void Start()
    {
        _mainCamera = Camera.main;
        _interactionIndicator.gameObject.SetActive(false);

        // 자동 등록
        _interactionObjects.AddRange(FindObjectsOfType<InteractionObject>());
    }

    private void Update()
    {
        UpdateCurrentTarget();
        UpdateUI();
    }

    void UpdateCurrentTarget()
    {
        _currentTarget = null;
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);

        foreach (var interaction in _interactionObjects)
        {
            if (interaction.stateEnum == _player.StateMachine.CurrentStateEnum)
                continue;

            if (CheckMousePos(interaction.Col))
            {
                _currentTarget = interaction;
                break;
            }
        }
    }

    void UpdateUI()
    {
        if (_currentTarget == null || !CheckMousePos(_currentTarget.Col))
        {
            if (_currentTarget != null)
                _currentTarget.interactionImg.enabled = false;

            _interactionIndicator.gameObject.SetActive(false);
            _currentTarget = null;
            return;
        }

        Vector3 screenPos = _mainCamera.WorldToViewportPoint(_currentTarget.interactionImg.transform.position);
        bool isVisible = screenPos.z > 0 && screenPos.x > 0 && screenPos.x < 1 && screenPos.y > 0 && screenPos.y < 1;

        if (isVisible)
        {
            _currentTarget.interactionImg.enabled = true;
            _currentTarget.interactionImg.transform.LookAt(_mainCamera.transform);
            _interactionIndicator.gameObject.SetActive(false);
        }
        else
        {
            _currentTarget.interactionImg.enabled = false;
            ShowOffScreenIndicator(_currentTarget);
        }
    }

    void ShowOffScreenIndicator(InteractionObject obj)
    {
        Vector3 screenPos = _mainCamera.WorldToScreenPoint(obj.TargetPos);
        Vector3 clamped = screenPos;
        clamped.x = Mathf.Clamp(clamped.x, 50, Screen.width - 50);
        clamped.y = Mathf.Clamp(clamped.y, 50, Screen.height - 50);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _canvasRect, clamped, _mainCamera, out Vector2 localPoint);

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
