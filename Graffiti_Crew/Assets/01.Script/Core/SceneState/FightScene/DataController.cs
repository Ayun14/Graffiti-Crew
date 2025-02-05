using AH.Map;
using UnityEngine;

public class DataController : Observer<GameStateController>
{
    [SerializeField] private LoadStageSO _stageSO;

    private StageDataSO _stageData;

    private bool _isFight = false;
    private float _currentDrawingTime = 0;

    private void Awake()
    {
        Attach();
    }

    private void Update()
    {
        if (_isFight) _currentDrawingTime += Time.deltaTime;
    }

    private void OnDestroy()
    {
        Detach();
    }

    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            if (mySubject.GameState == GameState.Loding)
            {
                FindDatas();
                GiveData();
                _currentDrawingTime = 0;
            }

            _isFight = mySubject.GameState == GameState.Fight;

            if (_stageData != null)
            {
                if (mySubject.GameState == GameState.Finish)
                {
                    _stageData.stageResult.drawingTime = (int)_currentDrawingTime;
                    int star = _stageData.stageResult.CalculationStar(_stageData.minCombo, _stageData.maxNodeFalse, _stageData.mindrawingTime);

                    Debug.Log("star : " + star);
                    if (star > _stageData.stageSaveData.star)
                    {
                        _stageData.stageSaveData.star = star;
                        Debug.Log("star 저장됨");
                    }
                }

                if (mySubject.GameState == GameState.Result)
                {
                    // 스테이지 클리어
                    _stageData.isClearStage = true;
                }
            }
        }
    }

    private void FindDatas()
    {
        _stageData = Resources.Load("StageData/" + _stageSO.GetLoadStageName()) as StageDataSO;
    }

    private void GiveData()
    {
        if (_stageData != null)
        {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("NeedLoding");

            foreach (GameObject obj in objects)
            {
                if (obj.TryGetComponent(out INeedLoding needLoding))
                    needLoding.LodingHandle(_stageData);
            }

            // 클리어 되지 않았을 때만 타임라인 재생
            GameState nextState = _stageData.isClearStage ? GameState.CountDown : GameState.Timeline;

            // Stage Result SO Reset
            _stageData.stageResult.Reset();

            mySubject.ChangeGameState(nextState);
        }
    }
}
