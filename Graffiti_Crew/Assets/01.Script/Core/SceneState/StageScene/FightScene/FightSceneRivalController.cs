using UnityEngine;
using System.Linq;

public class FightSceneRivalController : Observer<GameStateController>, INeedLoding
{
    [SerializeField] private SliderValueSO _rivalSliderValueSO;

    // Graffiti
    private Sprite _graffiti;
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
        _graffiti = dataController.stageData.rivalGraffiti;
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
            if (mySubject.GameState == GameState.Timeline)
            {
                AnimationEvent.SetAnimation?.Invoke(2, AnimationEnum.Ready);
            }
            else if (mySubject.GameState == GameState.Countdown)
            {
                RivalPositionToGraffiti();
            }
            else if (mySubject.GameState == GameState.Fight)
            {
                AnimationEvent.SetAnimation?.Invoke(2, AnimationEnum.Paint);
            }
            else if (mySubject.GameState == GameState.Finish)
            {
                SetGraffiti(null);
                _rival.position = _resultTrm.position;
                _rival.localRotation = _resultTrm.localRotation;
            }
        }
    }

    private void SetGraffiti(Sprite sprite)
    {
        _graffitiRenderer.sprite = _graffiti;
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

        float percent = _currentTime / _rivalDrawingTime;
        _rivalSliderValueSO.Value = _rivalSliderValueSO.max * percent;

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
        mySubject.SetWhoIsWin(false);
        mySubject.ChangeGameState(GameState.Finish);
    }
}

    #endregion
}
