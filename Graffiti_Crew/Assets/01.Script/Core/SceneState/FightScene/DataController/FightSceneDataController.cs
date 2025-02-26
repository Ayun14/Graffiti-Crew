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
        if (mySubject.GameState == GameState.Fight)
        {
            _currentDrawingTime = 0;
            _isFight = mySubject.GameState == GameState.Fight;
        }

        if (_stageData != null)
        {
            if (mySubject.GameState == GameState.Finish)
            {
                _stageData.stageResult.drawingTime = (int)_currentDrawingTime;
                int star = _stageData.stageResult.CalculationStar(_stageData.minCombo, _stageData.maxNodeFalse, _stageData.maxDrawingTime);

                Debug.Log("star : " + star);
                if (star > _stageData.stageSaveData.star)
                    _stageData.stageSaveData.star = star;
            }

            if (mySubject.GameState == GameState.Result)
            {
                // �������� Ŭ����
                _stageData.isClearStage = true;
                _stageData.stageSaveData.isClear = true;
            }
        }
    }

    protected override void GiveData()
    {
        base.GiveData();

        // Ŭ���� ���� �ʾ��� ���� Ÿ�Ӷ��� ���
        GameState nextState = _stageData.isClearStage ? GameState.CountDown : GameState.Timeline;

        // Stage Result SO Reset
        _stageData.stageResult.Reset();

        mySubject.ChangeGameState(nextState);
    }
}
