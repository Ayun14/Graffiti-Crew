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
                // 스테이지 클리어
                _stageData.isClearStage = true;
                _stageData.stageSaveData.isClear = true;
            }
        }
    }

    protected override void GiveData()
    {
        base.GiveData();

        // 클리어 되지 않았을 때만 타임라인 재생
        GameState nextState = _stageData.isClearStage ? GameState.CountDown : GameState.Timeline;

        // Stage Result SO Reset
        _stageData.stageResult.Reset();

        mySubject.ChangeGameState(nextState);
    }
}
