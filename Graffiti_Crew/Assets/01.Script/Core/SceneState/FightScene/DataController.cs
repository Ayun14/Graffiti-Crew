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
                        Debug.Log("star �����");
                    }
                }

                if (mySubject.GameState == GameState.Result)
                {
                    // �������� Ŭ����
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

            // Ŭ���� ���� �ʾ��� ���� Ÿ�Ӷ��� ���
            GameState nextState = _stageData.isClearStage ? GameState.CountDown : GameState.Timeline;

            // Stage Result SO Reset
            _stageData.stageResult.Reset();

            mySubject.ChangeGameState(nextState);
        }
    }
}
