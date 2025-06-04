using System.Threading.Tasks;
using UnityEngine;

public class StageSceneDataController : DataController
{
    private bool _isFight = false;
    private float _currentDrawingTime = 0;

    private void Update()
    {
        if (stageData.stageRuleType == StageRuleType.SpeedRule && _isFight) _currentDrawingTime += Time.deltaTime;
    }

    protected override void NotifyHandleChild()
    {
        if (mySubject != null)
        {
            _isFight = mySubject.GameState == GameState.Fight;

            if (stageData.stageRuleType == StageRuleType.SpeedRule && mySubject.GameState == GameState.Fight)
                _currentDrawingTime = 0;

            if (stageData != null)
            {
                if (mySubject.GameState == GameState.Finish)
                {
                    if (stageData.stageRuleType == StageRuleType.SpeedRule)
                        stageData.stageResult.value = (int)_currentDrawingTime;
                    GameEvents.SendFightGameResultEvent?.Invoke(stageData);
                }

                if (mySubject.GameState == GameState.Result)
                {
                    // 스테이지 클리어
                    stageData.isClearStage = true;
                    stageData.stageSaveData.stageState =StageState.Clear;
                }
            }
        }
    }

    protected override void FindDatas()
    {
        stageData = Resources.Load("StageData/" + stageSO.GetLoadStageName()) as StageDataSO;
    }

    protected async override void FinishGiveData()
    {
        // Stage Result SO Reset
        stageData.stageResult.Reset();
        mySubject.ChangeGameState(GameState.Timeline);

        PresentationEvents.SetFadeEvent?.Invoke(true);
        await Task.Delay(1100);
        PresentationEvents.FadeInOutEvent?.Invoke(true);
    }
}
