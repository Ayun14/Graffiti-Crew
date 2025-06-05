using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "StageResultSO", menuName = "SO/Stage/StageResultSO")]
public class StageResultSO : ScriptableObject
{
    public int value;
    public int coin = 0;

    public int minCost = 10;
    public int middleCost = 20;
    public int maxCost = 30;

    public void Reset()
    {
        value = 0;
        coin = 0;
    }

    public int CalculationStar() // 돈에 따른 금은동
    {
        int star = 0;
        if (minCost <= coin) ++star;
        if (middleCost <= coin) ++star;
        if (maxCost <= coin) ++star;
        return star;
    }


    public int CalculationCoin(int minStandard, int middleStandard, int maxStandard, StageRuleType stageRuleType) // value에 따른 돈 계산
    {
        if (stageRuleType == StageRuleType.PerfectRule) // 커야 좋음
        {
            if (minStandard <= value) coin += 10;
            if (middleStandard <= value) coin += 10;
            if (maxStandard <= value) coin += 10;
        }
        else // 작아야 좋음
        {
            if (value <= minStandard) coin += 10;
            if (value <= middleStandard) coin += 10;
            if (value <= maxStandard) coin += 10;
        }
        return coin;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        EditorUtility.SetDirty(this);
    }
#endif
}
