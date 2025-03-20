using Cysharp.Threading.Tasks.Triggers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SprayController : MonoBehaviour
{
    [Header("Shake")]
    [SerializeField] private SliderValueSO _shakeSliderValueSO;
    public bool isMustShakeSpray => isSprayCanShaking; //_shakeSliderValueSO.Value <= 0f;
    private bool isSprayCanShaking = false;

    [Header("Spray")]
    private Slider _spraySlider; // 곧 없앨거
    private RectTransform _spraySliderTrm; // 곧 없앨거
    [SerializeField] private SliderValueSO _spraySliderValueSO;
    public bool isSprayNone => _spraySliderValueSO.Value <= 0f;
    [SerializeField] private float _sprayAddAmount;

    #region Shake Settings
    [Header("Shake Settings")]
    [SerializeField] private float _shakeThreshold = 5f; // 흔들림 감지 값 (픽셀)
    [SerializeField] private int _shakeCountThreshold = 2; // 흔들림 횟수 값
    [SerializeField] private float _shakeDuration = 0.5f; // 흔들림 유지 시간
    [SerializeField] private float _idleDuration = 0.3f; // 움직임 멈춤을 감지하는 시간

    private float _shakeTimer = 0f; // 흔들림 지속 시간
    private float _idleTimer = 0f; // 움직임 없는 시간 누적
    private int _shakeCount = 0; // 흔들림 발생 횟수
    private bool _isShaking = false;
    private Vector3 _previousMousePos; // 이전 마우스 위치

    private float _currentDelayTime = 0;
    private float _shakeDelayTime = 0.4f;

    // Spray UI Animation
    private float _originYValue;
    private float _targetYValue;
    private Tween _jumpTween;
    #endregion

    private NodeJudgement _judgement;

    public void Init(NodeJudgement judgement)
    {
        _judgement = judgement;

        // Spray
        _spraySlider.value = 1f;
        _spraySliderValueSO.Value = _spraySliderValueSO.max;

        // Shake
        _shakeSliderValueSO.Value = _shakeSliderValueSO.max;

        ResetShakeDetection();
    }

    private void Awake()
    {
        // Spray UI
        Transform sprayPanelTrm = transform.Find("Panel_Spray");
        _spraySlider = sprayPanelTrm.GetComponentInChildren<Slider>();
        _spraySliderTrm = sprayPanelTrm.Find("Slider_Spray").GetComponent<RectTransform>();
        _originYValue = _spraySliderTrm.anchoredPosition.y;
        _targetYValue = _originYValue + 17f;
    }

    private void Update()
    {
        // shake 게이지 다 달았을 때만 흔들어야 한다고 표시
        if (isSprayCanShaking) ShakeSpray();
    }

    #region Shake

    private void ShakeSpray()
    {
        ShakeInput();

        if (_isShaking && _judgement.isNodeClick == false)
        {
            if (_currentDelayTime <= Time.time)
            {
                _currentDelayTime = Time.time + _shakeDelayTime;
                AddShakeAmount(_sprayAddAmount);
            }
        }
    }

    public void AddShakeAmount(float value)
    {
        if (_shakeSliderValueSO == null) return;

        _shakeSliderValueSO.Value += value; 

        if (_shakeSliderValueSO.Value <= 0f)
        {
            isSprayCanShaking = true;
            SprayUIAnimation();
        }
        else if (_shakeSliderValueSO.Value >= _shakeSliderValueSO.max)
        {
            isSprayCanShaking = false;
            if (_jumpTween != null) _jumpTween.Kill(); // 기존 Tween이 있다면 제거
        }
    }

    private void ShakeInput()
    {
        if (Input.GetMouseButton(0))
        {
            if (IsShaking())
            {
                if (!_isShaking)
                    _isShaking = true;

                _idleTimer = 0f;
            }
            else
            {
                _idleTimer += Time.deltaTime;

                if (_idleTimer >= _idleDuration && _isShaking)
                    ResetShakeDetection();
            }
        }
        else if (Input.GetMouseButtonUp(0))
            ResetShakeDetection();
    }

    private bool IsShaking()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        float distance = Vector3.Distance(_previousMousePos, currentMousePosition);

        if (distance > _shakeThreshold)
        {
            _shakeCount++;
            _shakeTimer += Time.deltaTime;
            _previousMousePos = currentMousePosition;

            if (_shakeCount >= _shakeCountThreshold && _shakeTimer >= _shakeDuration)
                return true;
        }

        _previousMousePos = currentMousePosition;
        return false;
    }

    private void ResetShakeDetection()
    {
        _shakeTimer = 0f;
        _idleTimer = 0f;
        _shakeCount = 0;
        _isShaking = false;

        _currentDelayTime = Time.time + _shakeDelayTime;
    }

    private void SprayUIAnimation()
    {
        if (_jumpTween != null) _jumpTween.Kill(); // 기존 Tween이 있다면 제거

        _jumpTween = _spraySliderTrm.DOAnchorPosY(_targetYValue, 0.3f)
            .SetEase(Ease.InQuart)
            .SetLoops(-1, LoopType.Yoyo); // 무한 반복 (Yoyo는 위아래 반복)
    }

    #endregion

    #region Spray

    public void AddSprayAmount(float value)
    {
        if (_spraySliderValueSO == null) return;

        float targetValue = _spraySliderValueSO.Value + value;
        DOTween.To(() => _spraySliderValueSO.Value,
            x => _spraySliderValueSO.Value = x, targetValue, 0.5f);

        // 없애야 하는거
        _spraySlider.value += value / _spraySliderValueSO.max;

        // Spray Empty
        if (_spraySlider.value <= 0f) // targetValue <= 0f
            _judgement.SprayEmptyEvent();
    }

    public void SprayChange()
    {
        AddSprayAmount(_spraySliderValueSO.max);
    }

    #endregion
}
