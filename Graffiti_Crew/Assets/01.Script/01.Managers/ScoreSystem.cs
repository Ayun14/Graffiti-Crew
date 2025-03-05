using AH.SaveSystem;
using System;
using UnityEngine;

public class ScoreSystem : MonoBehaviour {
    private void OnEnable() {
        Events.SendGameResultEvent += GameResult;
    }
    private void OnDisable() {
        Events.SendGameResultEvent -= GameResult;
    }

    private void GameResult(StageDataSO stageData) {
        int starCount = stageData.stageSaveData.star;
        //int combo = stageData.stageResult.comboCnt;
        //int time = stageData.stageResult.drawingTime;
        //int failCount = stageData.stageResult.nodeFalseCnt;

        //double score = ((time * 1.3) + combo - (failCount * 2)) * starCount;
        double score = 200 * starCount; // 여기 값 스테이지 별로 다르게 받아야해
        Debug.Log(score);
    }
}
