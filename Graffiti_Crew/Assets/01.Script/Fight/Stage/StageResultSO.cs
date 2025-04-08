using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "StageResultSO", menuName = "SO/Stage/StageResultSO")]
public class StageResultSO : ScriptableObject
{
    public int comboCnt = 0; // 모든 콤보 수
    public int nodeFalseCnt = 0; // 노드 실패 수
    public int drawingTime = 0; // graffiti를 그린 시간

    private int _star = 0;
    public int star
    {
        get => _star;
        set => _star = Mathf.Clamp(value, 0, 3);
    }

    public void Reset()
    {
        comboCnt = 0;
        nodeFalseCnt = 0;
        drawingTime = 0;

        star = 0;
    }

    public int CalculationStar(int data, int minCombo, int maxNodeFalse, int maxDrawingTime)
    {
        star = 0;

        if (data <= minCombo) ++star;
        if (data <= maxNodeFalse) ++star;
        if (data <= maxDrawingTime) ++star;

        return star;
    }
    public int CalculationStar(StageRuleType type, int minCombo, int maxNodeFalse, int maxDrawingTime) {
        star = 0;
        int data = 0;

        switch (type) {
            case StageRuleType.SpeedRule:
                data = drawingTime;
                break;
            case StageRuleType.PerfectRule:
                data = comboCnt;
                break;
            case StageRuleType.OneTouchRule:
                data = nodeFalseCnt;
                break;
        }

        if (data <= minCombo) ++star;
        if (data <= maxNodeFalse) ++star;
        if (data <= maxDrawingTime) ++star;

        return star;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        EditorUtility.SetDirty(this);
    }



#endif
}
