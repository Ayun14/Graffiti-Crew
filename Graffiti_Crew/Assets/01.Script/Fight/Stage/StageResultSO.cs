using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "StageResultSO", menuName = "SO/Stage/StageResultSO")]
public class StageResultSO : ScriptableObject
{
    public int value;
    public int coin = 0;

    private int minCost = 5;
    private int middleCost = 10;
    private int maxCost = 20;

    public void Reset()
    {
        value = 0;
        coin = 0;
    }

    public int CalculationStar() // ���� ���� ������
    {
        int star = 0;
        if (minCost <= coin)
            ++star;
        if (middleCost <= coin)
            ++star;
        if (maxCost <= coin)
            ++star;
        return star;
    }


    public int CalculationCoin(int minStandard, int middleStandard, int maxStandard, StageRuleType stageRuleType) // value�� ���� �� ���
    {
        if (stageRuleType == StageRuleType.PerfectRule) // Ŀ�� ����
        {
            if (minStandard <= value) coin += 5;
            if (middleStandard <= value) coin += 5;
            if (maxStandard <= value) coin += 10;
        }
        else // �۾ƾ� ����
        {
            if (value <= minStandard) coin += 5;
            if (value <= middleStandard) coin += 5;
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
