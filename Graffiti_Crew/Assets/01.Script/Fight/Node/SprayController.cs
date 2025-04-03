using DG.Tweening;
using System.Collections;
using UnityEngine;

public class SprayController : MonoBehaviour
{
    [Header("Shake")]
    [SerializeField] private SliderValueSO _shakeSliderValueSO;
    [SerializeField] private Texture2D _sprayCursor;
    [SerializeField] private Texture2D _shakeSprayCursor;
    public bool isMustShakeSpray => isSprayCanShaking; //_shakeSliderValueSO.Value <= 0f;
    private bool isSprayCanShaking = false;
    private bool _isShaking = false;

    [Header("Spray")]
    [SerializeField] private SliderValueSO _spraySliderValueSO;
    public bool isSprayNone => _spraySliderValueSO.Value <= 0f;
    [SerializeField] private float _sprayAddAmount;

    private NodeJudgement _judgement;

    public void Init(NodeJudgement judgement)
    {
        _judgement = judgement;

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
        if (_judgement.isNodeClick) return;

        if (_isShaking == false && Input.GetMouseButtonDown(0))
        {
            _isShaking = true;
            AddShakeAmount(_sprayAddAmount);

            //Sound
            GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.Spray_Shake);
        }
        
        if (_isShaking == true && Input.GetMouseButtonDown(1))
        {
            _isShaking = false;
            AddShakeAmount(_sprayAddAmount);

            //Sound
            GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.Spray_Shake);
        }
    }

    public void AddShakeAmount(float value)
    {
        if (_shakeSliderValueSO == null) return;

        _shakeSliderValueSO.Value += value;

        // Shaking
        if (_shakeSliderValueSO.Value <= 0f)
        {
            isSprayCanShaking = true;
            Cursor.SetCursor(_shakeSprayCursor, new Vector2(0, 0), CursorMode.Auto);
        }
        else if (_shakeSliderValueSO.Value >= _shakeSliderValueSO.max)
        {
            isSprayCanShaking = false;
            Cursor.SetCursor(_sprayCursor, new Vector2(0, 0), CursorMode.Auto);
        }
    }

    #endregion

    #region Spray

    public void AddSprayAmount(float value)
    {
        if (_spraySliderValueSO == null) return;

        float targetValue = _spraySliderValueSO.Value + value;
        DOTween.To(() => _spraySliderValueSO.Value,
            x => _spraySliderValueSO.Value = x, targetValue, 0.5f);

        // Spray Empty
        if (targetValue <= 0f)
        {
            StartCoroutine(SprayEmpty());
        }
    }

    private IEnumerator SprayEmpty()
    {
        // 스프레이 통 떨구기
        Cursor.SetCursor(_shakeSprayCursor, new Vector2(0, 0), CursorMode.Auto);
        yield return new WaitForSeconds(1f);
        Cursor.SetCursor(_sprayCursor, new Vector2(0, 0), CursorMode.Auto);
        ResetSpray();
    }

    private void ResetSpray()
    {
        AddSprayAmount(_spraySliderValueSO.max);
        AddShakeAmount(_shakeSliderValueSO.max);
    }

    #endregion
}
