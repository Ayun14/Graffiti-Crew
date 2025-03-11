using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class RivalController : Observer<GameStateController>, INeedLoding
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

    public void LodingHandle(StageDataSO stageData)
    {
        _graffiti = stageData.rivalGraffiti;
        _rivalDrawingTime = stageData.rivalClearTime;
        _rival = Instantiate(stageData.rivalPrefab, Vector3.zero, Quaternion.identity, transform).transform;
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
        _rivalCheckPercent = Random.Range(45, 70);
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
            if (mySubject.GameState == GameState.CountDown)
            {
                _rival.position = _graffitiTrm.position;
                _rival.localRotation = _graffitiTrm.localRotation;
            }

            _isFight = mySubject.GameState == GameState.Fight;

            if (mySubject.GameState == GameState.Finish)
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
