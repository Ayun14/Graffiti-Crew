using AH.Save;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using UnityEngine;

public class StageSceneDataController : DataController
{
    private bool _isFight = false;
    private float _currentDrawingTime = 0;

    [SerializeField] private StringSaveDataSO _gameProgressSO;

    private void Update()
    {
        if (_isFight && stageData.stageRuleType == StageRuleType.SpeedRule) _currentDrawingTime += Time.deltaTime;
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
                    stageData.isPlayerWin = mySubject.IsPlayerWin;
                    stageData.stageSaveData.stageState = mySubject.IsPlayerWin ? StageState.Clear : StageState.CanPlay;
                    
                    if (stageData.stageSaveData.stageState == StageState.Clear) { // 클리어시 게임 진행도 저장할 수 있도록
                        SaveGameProgress();
                    }
                }
            }
        }
    }

    private void SaveGameProgress() {
        string chapter = stageSO.chapter; 
        string chapterNumberStr = chapter.Replace("Chapter", "");
        int chapterNumber = int.Parse(chapterNumberStr);

        string stageNumber = stageSO.GetStageNumber(); // 예: "Chapter1Battle1"

        string progress = $"{chapterNumber}-{stageNumber}";
        _gameProgressSO.data = progress;
    }

    protected override void FindDatas()
    {
        stageData = Resources.Load("StageData/" + stageSO.GetLoadStageName()) as StageDataSO;
    }

    protected async override void FinishGiveData()
    {
        // Stage Result SO Reset
        stageData.stageResult.Reset();
        stageData.isPlayerWin = false;
        mySubject.ChangeGameState(GameState.Timeline);

        PresentationEvents.SetFadeEvent?.Invoke(true);
        await Task.Delay(1100);
        PresentationEvents.FadeInOutEvent?.Invoke(true);
    }
}
