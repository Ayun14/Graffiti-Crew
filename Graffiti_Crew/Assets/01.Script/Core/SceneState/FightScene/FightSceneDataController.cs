using System.Threading.Tasks;
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
                    //int star = stageData.stageResult.CalculationStar(stageData.minCombo, stageData.maxNodeFalse, stageData.maxDrawingTime);
                    //Debug.Log("star : " + star);
                    //if (star > stageData.stageSaveData.star)
                    //    stageData.stageSaveData.star = star;
                    GameEvents.SendFightGameResultEvent?.Invoke(stageData);
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

    protected async override void FinishGiveData()
    {
        // Stage Result SO Reset
        stageData.stageResult.Reset();
        mySubject.ChangeGameState(GameState.Timeline); // ui�ε� �����

        PresentationEvents.SetFadeEvent?.Invoke(true);
        await Task.Delay(1100);
        PresentationEvents.FadeInOutEvent?.Invoke(true);
    }
}
