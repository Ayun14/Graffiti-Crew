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
                // �������� Ŭ����
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

            // Ŭ���� ���� �ʾ��� ���� Ÿ�Ӷ��� ���
            GameState nextState = _stageData.isClearStage ? GameState.CountDown : GameState.Timeline;
            mySubject.ChangeGameState(nextState);
        }
    }
}
