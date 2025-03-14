using UnityEngine;

public class FightSceneDataController : DataController
{
    private bool _isFight = false;
    private float _currentDrawingTime = 0;

    private void Update()
    {
        if (_isFight) _currentDrawingTime += Time.deltaTime;
    }

    protected override void NotifyHandleChild()
    {
        if (mySubject != null)
        {
            if (mySubject.GameState == GameState.Fight)
            {
                _currentDrawingTime = 0;
                _isFight = mySubject.GameState == GameState.Fight;
            }

            if (stageData != null)
            {
                if (mySubject.GameState == GameState.Finish)
                {
                    stageData.stageResult.drawingTime = (int)_currentDrawingTime;
                    int star = stageData.stageResult.CalculationStar(stageData.minCombo, stageData.maxNodeFalse, stageData.maxDrawingTime);
                    GameEvents.SendGameResultEvent?.Invoke(stageData);
                    Debug.Log("star : " + star);
                    if (star > stageData.stageSaveData.star)
                        stageData.stageSaveData.star = star;
                }

                if (mySubject.GameState == GameState.Result)
                {
                    // �������� Ŭ����
                    stageData.isClearStage = true;
                    stageData.stageSaveData.isClear = true;
                }
            }
        }
    }

    protected override void FindDatas()
    {
        stageData = Resources.Load("StageData/" + stageSO.GetLoadStageName()) as StageDataSO;
    }

    protected override void FinishGiveData()
    {
        // Ŭ���� ���� �ʾ��� ���� Ÿ�Ӷ��� ���
        GameState nextState = stageData.isClearStage ? GameState.CountDown : GameState.Timeline;

        // Stage Result SO Reset
        stageData.stageResult.Reset();

        mySubject.ChangeGameState(nextState);
    }
}
