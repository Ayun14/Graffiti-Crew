using UnityEngine;

public class ScoreSystem : MonoBehaviour {
    private void OnEnable() {
        GameEvents.SendFightGameResultEvent += FightGameResult;
    }
    private void OnDisable() {
        GameEvents.SendFightGameResultEvent -= FightGameResult;
    }

    private void FightGameResult(StageDataSO stageData) {
        Debug.Log("result");
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
