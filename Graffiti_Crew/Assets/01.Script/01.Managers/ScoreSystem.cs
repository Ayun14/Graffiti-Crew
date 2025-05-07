using System;
using UnityEngine;

public class ScoreSystem : MonoBehaviour {
    private void OnEnable() {
        GameEvents.SendFightGameResultEvent += FightGameResult;
    }
    private void OnDisable() {
        GameEvents.SendFightGameResultEvent -= FightGameResult;
    }

    // TODO : 기획에 따라 바뀌여서 계산 함수 교체해야함
    private void FightGameResult(StageDataSO stageData) {
        CalStar(stageData);

        int score = 0;
        int combo = stageData.stageResult.value;
        int failCount = stageData.stageResult.value;
        int increase = 1; // 기본값
        int decrease = 1; // 기본값

        switch (stageData.stagetype) {
            case StageType.Stage:
                increase = 1;
                decrease = 2;
                break;
            case StageType.Activity:
                increase = 2;
                decrease = 1;
                break;
        }
        if (failCount !=0 ) {
            score = increase * ((combo / decrease) / failCount);
        }
        else {
            score = increase * (combo / decrease);
        }
            CoinSystem.AddCoin(score);
    }

    private void CalStar(StageDataSO stageData) {
        int star = 0; 
        star = stageData.stageResult.CalculationStar
            (stageData.minStandard, stageData.middleStandard, stageData.maxStandard);

        Debug.Log("star : " + star);
        if (star > stageData.stageSaveData.star)
            stageData.stageSaveData.star = star;
    }
}
