using UnityEngine;

public class RivalController : Observer<GameStateController>, INeedLoding
{
    [SerializeField] private SliderValueSO _rivalSliderValueSO;

    private Sprite _graffiti;
    private SpriteRenderer _graffitiRenderer;

    private bool _isFight = false;

    private float _rivalDrawingTime;
    private float _currentTime = 0;

    public void LodingHandle(StageDataSO stageData)
    {
        _graffiti = stageData.rivalGraffiti;
        _rivalDrawingTime = stageData.rivalClearTime;
    }

    private void Awake()
    {
        Attach();

        _graffitiRenderer = GetComponentInChildren<SpriteRenderer>();
        _graffitiRenderer.sprite = null;

        _rivalSliderValueSO.value = 0;

        _isFight = false;
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

    private void RivalProgressSliderUpdate()
    {
        if (_isFight) return;

        _currentTime += Time.deltaTime;
        _rivalSliderValueSO.value = _currentTime / _rivalDrawingTime;
    }

    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            _isFight = mySubject.GameState == GameState.Fight;

            if (mySubject.GameState == GameState.Finish)
                SetGraffiti(null);
        }
    }

    private void SetGraffiti(Sprite sprite)
    {
        _graffitiRenderer.sprite = _graffiti;
    }
}
