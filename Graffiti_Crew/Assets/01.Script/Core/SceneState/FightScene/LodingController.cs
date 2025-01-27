using AH.Map;
using UnityEngine;

public class LodingController : Observer<GameStateController>
{
    [SerializeField] private LoadStageSO _stageSO;

    private StageDataSO _stageData;

    private void Awake()
    {
        Attach();
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
                FindStageData();
                GiveData();
            }

            if (mySubject.GameState == GameState.Result && _stageData != null)
            {
                // 스테이지 클리어
                _stageData.isClearStage = true;
            }
        }
    }

    private void FindStageData()
    {
        _stageData = Resources.Load(_stageSO.GetLoadStageName()) as StageDataSO;
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
            mySubject.ChangeGameState(nextState);
        }
    }
}
