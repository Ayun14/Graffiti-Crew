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
                    
                    if (stageData.stageSaveData.stageState == StageState.Clear) { // Ŭ����� ���� ���൵ ������ �� �ֵ���
                        SaveGameProgress();
                    }
                }
            }
        }
    }

    private void SaveGameProgress() {
        string stageName = stageSO.GetCurrentStageName(); // ��: "Chapter1Battle1"
        string progress = "";

        Match match = Regex.Match(stageName, @"Chapter(\d+)(Battle|Activity)(\d+)");
        if (match.Success) {
            string chapterNumber = match.Groups[1].Value;
            string stageType = match.Groups[2].Value;
            string stageNumber = match.Groups[3].Value;

            string typeKorean = stageType == "Battle" ? "���" : "Ȱ��";

            progress = $"é��{chapterNumber} {typeKorean}{stageNumber}";
            _gameProgressSO.data = progress;
        }
        else {
            Debug.LogWarning($"Stage �̸� �Ľ� ����: {stageName}");
            _gameProgressSO.data = "�� �� ����";
        }

        Debug.Log(_gameProgressSO.data);
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
