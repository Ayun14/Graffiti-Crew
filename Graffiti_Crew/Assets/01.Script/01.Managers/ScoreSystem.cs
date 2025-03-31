using AH.SaveSystem;
using System;
using UnityEngine;

public class ScoreSystem : MonoBehaviour {
    private void OnEnable() {
        GameEvents.SendGameResultEvent += GameResult;
    }
    private void OnDisable() {
        GameEvents.SendGameResultEvent -= GameResult;
    }

    private void GameResult(StageDataSO stageData) {
        int combo = stageData.stageResult.comboCnt;
        int failCount = stageData.stageResult.nodeFalseCnt;
        int score = 0;

        if (failCount != 0) {
            score = (combo / 2) / failCount;
        }
        else {
            score = combo / 2;
        }
        CoinSystem.AddCoin(score);
    }
}
