using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SprayController : MonoBehaviour
{
    private Slider _sprayAmountSlider;
    private Slider _shakeAmountSlider;

    public bool isMustShakeSpray => _sprayAmountSlider.value == 0f;
    public bool isSprayNone => _shakeAmountSlider.value == 0f;

    [Header("Spray")]
    [SerializeField] private float _sprayAddAmount;

    #region Shake Settings
    [Header("Shake Settings")]
    [SerializeField] private float _shakeThreshold = 5f; // ��鸲 ���� �� (�ȼ�)
    [SerializeField] private int _shakeCountThreshold = 2; // ��鸲 Ƚ�� ��
    [SerializeField] private float _shakeDuration = 0.5f; // ��鸲 ���� �ð�
    [SerializeField] private float _idleDuration = 0.3f; // ������ ������ �����ϴ� �ð�

    private float _shakeTimer = 0f; // ��鸲 ���� �ð�
    private float _idleTimer = 0f; // ������ ���� �ð� ����
    private int _shakeCount = 0; // ��鸲 �߻� Ƚ��
    private bool _isShaking = false;
    private Vector3 _previousMousePos; // ���� ���콺 ��ġ

    private float _currentDelayTime = 0;
    private float _shakeDelayTime = 1f;
    #endregion

    private NodeJudgement _judgement;

    private void Awake()
    {
        Transform parent = transform.Find("Panel_Sliders");
        _sprayAmountSlider = parent.Find("Slider_SprayAmount").GetComponent<Slider>();
        _shakeAmountSlider = parent.Find("Slider_ShakeAmount").GetComponent<Slider>();
    }

    public void Init(NodeJudgement judgement, int sprayAmount, int shakeAmount)
    {
        _judgement = judgement;

        if (_sprayAmountSlider == null || _shakeAmountSlider == null) return;

        // Spray
        _sprayAmountSlider.maxValue = sprayAmount;
        _sprayAmountSlider.value = sprayAmount;

        // Shake
        _shakeAmountSlider.maxValue = shakeAmount;
        _shakeAmountSlider.value = shakeAmount;

        ResetShakeDetection();
    }

    private void Update()
    {
        ShakeSpray();
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
        if (_shakeAmountSlider == null) return;

        float targetValue = _shakeAmountSlider.value + value;
        DOTween.To(() => _shakeAmountSlider.value, 
            x => _shakeAmountSlider.value = x, targetValue, 0.5f);
        
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

    #endregion

    #region Spray

    public void AddSprayAmount(float value)
    {
        if (_sprayAmountSlider == null) return;

        float targetValue = _sprayAmountSlider.value + value;
        DOTween.To(() => _sprayAmountSlider.value,
            x => _sprayAmountSlider.value = x, targetValue, 0.5f);

        if (_sprayAmountSlider.value < 0f)
        {
            Debug.LogWarning("Spray ��� ����");
        }
    }

    #endregion
}
