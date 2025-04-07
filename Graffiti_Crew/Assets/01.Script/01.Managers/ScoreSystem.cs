using System;
using UnityEngine;

public class ScoreSystem : MonoBehaviour {
    private void OnEnable() {
        GameEvents.SendFightGameResultEvent += FightGameResult;
    }
    private void OnDisable() {
        GameEvents.SendFightGameResultEvent -= FightGameResult;
    }

    private void FightGameResult(StageDataSO stageData) {
        CalStar(stageData);

        int score = 0;
        int combo = stageData.stageResult.comboCnt;
        int failCount = stageData.stageResult.nodeFalseCnt;
        int increase = 1; // 기본값
        int decrease = 1; // 기본값

        switch (stageData.stagetype) {
            case StageType.Stage:
                increase = 1;
                decrease = 2;
                break;
            case StageType.Request:
                increase = 2;
                decrease = 1;
                break;
        }
        score = increase * ((combo / decrease) / failCount);
        CoinSystem.AddCoin(score);
    }

    private void CalStar(StageDataSO stageData) {
        int star = 0;
        switch (stageData.stageRuleType) {
            case StageRuleType.SpeedRule:
                star = stageData.stageResult.CalculationSpeedRuleStar(stageData.minStandard, stageData.middleStandard, stageData.maxStandard);
                break;
            case StageRuleType.PerfectRule:
                star = stageData.stageResult.CalculationPerfectRuleStar(stageData.minStandard, stageData.middleStandard, stageData.maxStandard);
                break;
            case StageRuleType.OneTouchRule:
                star = stageData.stageResult.CalculationOneTouchRuleStar(stageData.minStandard, stageData.middleStandard, stageData.maxStandard);
                break;
        }
        Debug.Log("star : " + star);
        if (star > stageData.stageSaveData.star)
            stageData.stageSaveData.star = star;
    }
}
