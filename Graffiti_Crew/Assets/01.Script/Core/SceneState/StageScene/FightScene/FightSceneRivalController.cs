using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FightSceneRivalController : Observer<GameStateController>, INeedLoding
{
    [SerializeField] private SliderValueSO _rivalSliderValueSO;
    private Tween _rivalProgressValueChangeTween;
    // Graffiti
    private List<Sprite> _graffitis = new();
    private SpriteRenderer _graffitiRenderer;

    // Game Object
    private Transform _rival;
    private Transform _graffitiTrm;
    private Transform _resultTrm;

    private bool _isFight = false;
    private bool _isCompleteRivalCheck = false;

    private int _rivalCheckPercent;

    private float _rivalDrawingTime;
    private float _currentTime = 0;

    public void LodingHandle(DataController dataController)
    {
        _graffitis = dataController.stageData.rivalGraffiti;
        _rivalDrawingTime = dataController.stageData.rivalClearTime;
        _rival = Instantiate(dataController.stageData.rivalPrefabList.First().gameObject, _resultTrm.position, _resultTrm.localRotation, transform).transform;

        dataController.SuccessGiveData();
    }

    private void Awake()
    {
        Attach();

        _graffitiTrm = transform.Find("GraffitiPos").GetComponent<Transform>();
        _resultTrm = transform.Find("ResultPos").GetComponent<Transform>();

        _graffitiRenderer = GetComponentInChildren<SpriteRenderer>();
        _graffitiRenderer.sprite = null;

        _rivalSliderValueSO.Value = _rivalSliderValueSO.min;

        _isFight = false;
        _isCompleteRivalCheck = false;
        _rivalCheckPercent = Random.Range(60, 80);
        _currentTime = 0;
    }

    private void OnDestroy()
    {
        Detach();
    }

    private void Update()
    {
        RivalProgressSliderUpdate();
    }

    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            _isFight = mySubject.GameState == GameState.Fight;
            if (mySubject.GameState == GameState.Timeline)
            {
                AnimationEvent.SetAnimation?.Invoke(2, AnimationEnum.Ready);
            }
            else if (mySubject.GameState == GameState.Countdown)
            {
                RivalPositionToGraffiti();
            }
            else if (_isFight)
            {
                AnimationEvent.SetAnimation?.Invoke(2, AnimationEnum.Paint);
            }
            else if (mySubject.GameState == GameState.Finish)
            {
                _rival.position = _resultTrm.position;
                _rival.localRotation = _resultTrm.localRotation;
            }
        }
    }

    private void SetGraffiti()
    {
        int idx = (int)(_currentTime / (_rivalDrawingTime / 3));
        if (idx < _graffitis.Count)
            _graffitiRenderer.sprite = _graffitis[idx];
    }

    private void RivalPositionToGraffiti()
    {
        _rival.position = _graffitiTrm.position;
        _rival.localRotation = _graffitiTrm.localRotation;
    }

    public void WaitAnimation()
    {
        AnimationEvent.SetAnimation?.Invoke(2, AnimationEnum.Idle);
    }

    public void WinLoseAnimation()
    {
        if (mySubject.IsPlayerWin)
            AnimationEvent.SetAnimation?.Invoke(2, AnimationEnum.Lose);
        else
            AnimationEvent.SetAnimation?.Invoke(2, AnimationEnum.Win);
    }

    #region Slider

    private void RivalProgressSliderUpdate()
    {
        if (!_isFight) return;

        if (_rivalDrawingTime >= _currentTime)
        {
            _currentTime += Time.deltaTime;

            if (_rivalSliderValueSO == null) return;
            if (_rivalProgressValueChangeTween != null && _rivalProgressValueChangeTween.IsActive())
                _rivalProgressValueChangeTween.Complete();

            float percent = _currentTime / _rivalDrawingTime;
            float targetValue = _rivalSliderValueSO.Value + percent;

            _rivalProgressValueChangeTween = DOTween.To(() => _rivalSliderValueSO.Value,
                x => _rivalSliderValueSO.Value = x, targetValue, 0.2f);

            RivalCheck();
            FinishCheck();
        }
    }

    private void RivalCheck()
    {
        if (_isCompleteRivalCheck) return;

        if (_rivalSliderValueSO.Value > _rivalCheckPercent)
        {
            _isCompleteRivalCheck = true;
            mySubject.OnRivalCheckEvent();
        }
    }

    private void FinishCheck()
    {
        if (_rivalSliderValueSO.Value >= _rivalSliderValueSO.max)
        {
            SetGraffiti();
            mySubject.SetWhoIsWin(false);
            mySubject.ChangeGameState(GameState.Finish);
        }
    }

    #endregion
}
