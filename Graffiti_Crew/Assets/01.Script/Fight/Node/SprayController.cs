using AH.UI.Events;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SprayController : MonoBehaviour
{
    public UnityEvent OnSprayNone;
    public UnityEvent OnAirNone;

    [Header("Cursor")]
    [SerializeField] private Texture2D _sprayCursor;
    [SerializeField] private Texture2D _noneSprayCursor;
    [SerializeField] private List<Texture2D> _lodingCursors = new();

    [Header("Shake")]
    [SerializeField] private SliderValueSO _shakeSliderValueSO;
    [SerializeField] private float _sprayAddAmount;
    public bool isMustShakeSpray => isSprayCanShaking; //_shakeSliderValueSO.Value <= 0f;
    private bool isSprayCanShaking = false;
    private bool _isShaking = false; 
    private Tween _shakeValueChangeTween;

    [Header("Spray")]
    [SerializeField] private SliderValueSO _spraySliderValueSO;
    [SerializeField] private SliderValueSO _sprayCanValueSO;
    [SerializeField] private float _maxSprayValue;
    [SerializeField] private GameObject _sprayCanPrefab;
    [SerializeField] private float _forcePower = 2f;
    [SerializeField] private float _sprayChangeTime = 2f;
    private Tween _sprayValueChangeTween;

    protected StageGameRule _stageGameRule;

    public void Init(StageGameRule stageGameRule)
    {
        _stageGameRule = stageGameRule;

        // Spray
        _spraySliderValueSO.Value = _spraySliderValueSO.max;

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

        // Shaking
        if (targetValue <= _shakeSliderValueSO.min)
        {
            isSprayCanShaking = true;
            OnAirNone?.Invoke();
            SetCursor(_spraySliderValueSO.Value, targetValue);
        }
        else if (targetValue >= _shakeSliderValueSO.max)
        {
            isSprayCanShaking = false;
            SetCursor(_spraySliderValueSO.Value, targetValue);
        }
    }

    #endregion

    #region Spray

    public void AddSprayAmount(float value)
    {
        if (_spraySliderValueSO == null) return;
        if (_sprayValueChangeTween != null && _sprayValueChangeTween.IsActive())
            _sprayValueChangeTween.Complete();

        float targetValue = _spraySliderValueSO.Value + value;
        _sprayValueChangeTween = DOTween.To(() => _spraySliderValueSO.Value,
            x => _spraySliderValueSO.Value = x, targetValue, 0.1f)
            .OnComplete(() => {
                StageEvent.ChangeSprayValueEvent?.Invoke();
            });

        // Spray Empty
        if (_stageGameRule.GetSprayEmpty() == false && targetValue <= _spraySliderValueSO.min)
        {
            OnSprayNone?.Invoke();
            StartCoroutine(SprayEmpty());
            _stageGameRule.SetSprayEmpty(true);
        }
    }

    private IEnumerator SprayEmpty()
    {
        // Spray Can
        if (--_sprayCanValueSO.Value == _sprayCanValueSO.min)
            _stageGameRule.PlayerLoseCheck();

        // 스프레이 통 떨구기
        Rigidbody rigid = Instantiate(_sprayCanPrefab, transform).GetComponent<Rigidbody>();
        Vector3 randomOffset = new Vector3(
            Random.Range(-0.5f, 0.5f),
            Random.Range(-0.2f, 0.2f),
            Random.Range(0.1f, 0.3f));
        Vector3 finalDirection = (transform.forward + randomOffset).normalized;
        rigid.AddForce(randomOffset * _forcePower, ForceMode.Impulse);

        // Sound
        GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Spray_Change);

        // Loding Cursor
        for (int i = 0; i < 8; ++i)
        {
            SetCursor(_lodingCursors[i % 4]);
            yield return new WaitForSeconds(_sprayChangeTime / 8f);
        }

        ResetSpray();
        SetCursor(_sprayCursor);
        _stageGameRule.SetSprayEmpty(false);
    }

    private void ResetSpray()
    {
        AddSprayAmount(_spraySliderValueSO.max);
        AddShakeAmount(_shakeSliderValueSO.max);
    }

    #endregion

    private void SetCursor(float sprayValue, float shakeValue)
    {
        if (sprayValue > 0 && shakeValue > 0)
            Cursor.SetCursor(_sprayCursor, new Vector2(0, 0), CursorMode.Auto);
        else
            Cursor.SetCursor(_noneSprayCursor, new Vector2(0, 0), CursorMode.Auto);
    }

    private void SetCursor(Texture2D cursor)
    {
        if (cursor != null)
            Cursor.SetCursor(cursor, new Vector2(0, 0), CursorMode.Auto);
    }
}
