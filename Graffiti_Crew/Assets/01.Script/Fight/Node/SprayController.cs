using AH.UI.Events;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class SprayController : MonoBehaviour
{
    [Header("Shake")]
    [SerializeField] private SliderValueSO _shakeSliderValueSO;
    [SerializeField] private Texture2D _sprayCursor;
    [SerializeField] private Texture2D _shakeSprayCursor;
    [SerializeField] private float _sprayAddAmount;
    public bool isMustShakeSpray => isSprayCanShaking; //_shakeSliderValueSO.Value <= 0f;
    private bool isSprayCanShaking = false;
    private bool _isShaking = false; 
    private Tween _shakeValueChangeTween;

    [Header("Spray")]
    [SerializeField] private float _maxSprayValue;
    [SerializeField] private GameObject _sprayCanPrefab;
    [SerializeField] private float _forcePower = 2f;
    [SerializeField] private float _sprayChangeTime = 2f;
    private float _currentSprayValue = 0;
    public float CurrentSparyValue
    {
        get { return _currentSprayValue; }
        set { _currentSprayValue = Mathf.Clamp(value, 0, _maxSprayValue); }
    }
    public bool isSprayNone => _currentSprayValue <= 0f;

    protected StageGameRule _stageGameRule;

    public void Init(StageGameRule stageGameRule)
    {
        _stageGameRule = stageGameRule;

        // Spray
        CurrentSparyValue = _maxSprayValue;

        // Shake
        _shakeSliderValueSO.Value = _shakeSliderValueSO.max;
    }

    private void Update()
    {
        // shake 게이지 다 달았을 때만 흔들어야 한다고 표시
        if (isSprayCanShaking) ShakeSpray();
    }

    #region Shake

    private void ShakeSpray()
    {
        //if (_judgement.isNodeClick) return;

        if (_isShaking == false && Input.GetMouseButtonDown(0))
        {
            _isShaking = true;
            AddShakeAmount(_sprayAddAmount);

            //Sound
            GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Spray_Shake);
        }
        
        if (_isShaking == true && Input.GetMouseButtonDown(1))
        {
            _isShaking = false;
            AddShakeAmount(_sprayAddAmount);

            //Sound
            GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Spray_Shake);
        }
    }

    public void AddShakeAmount(float value)
    {
        if (_shakeSliderValueSO == null) return;
        if (_shakeValueChangeTween != null && _shakeValueChangeTween.IsActive())
            _shakeValueChangeTween.Complete();

        float targetValue = _shakeSliderValueSO.Value + value;
        _shakeValueChangeTween = DOTween.To(() => _shakeSliderValueSO.Value,
            x => _shakeSliderValueSO.Value = x, targetValue, 0.1f);
        StageEvent.ChangeSprayValueEvent?.Invoke();

        // Shaking
        if (targetValue <= 0f)
        {
            isSprayCanShaking = true;
            Cursor.SetCursor(_shakeSprayCursor, new Vector2(0, 0), CursorMode.Auto);
        }
        else if (targetValue >= _shakeSliderValueSO.max)
        {
            isSprayCanShaking = false;
            Cursor.SetCursor(_sprayCursor, new Vector2(0, 0), CursorMode.Auto);
        }
    }

    #endregion

    #region Spray

    public void AddSprayAmount(float value)
    {
        CurrentSparyValue += value;

        // Spray Empty
        if (CurrentSparyValue <= 0f)
        {
            StartCoroutine(SprayEmpty());
            _stageGameRule.SetSprayEmpty(true);
        }
    }

    private IEnumerator SprayEmpty()
    {
        // 스프레이 통 떨구기
        Rigidbody rigid = Instantiate(_sprayCanPrefab, transform).GetComponent<Rigidbody>();
        Vector3 randomOffset = new Vector3(
            Random.Range(-0.5f, 0.5f),
            Random.Range(-0.2f, 0.2f),
            Random.Range(0.1f, 0.3f));
        Vector3 finalDirection = (transform.forward + randomOffset).normalized;
        rigid.AddForce(randomOffset * _forcePower, ForceMode.Impulse);
        Cursor.SetCursor(_shakeSprayCursor, new Vector2(0, 0), CursorMode.Auto);

        yield return new WaitForSeconds(_sprayChangeTime);

        Cursor.SetCursor(_sprayCursor, new Vector2(0, 0), CursorMode.Auto);
        ResetSpray();
        _stageGameRule.SetSprayEmpty(false);
    }

    private void ResetSpray()
    {
        AddSprayAmount(_maxSprayValue);
        AddShakeAmount(_shakeSliderValueSO.max);
    }

    #endregion
}
