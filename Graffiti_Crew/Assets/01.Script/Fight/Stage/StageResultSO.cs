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

    public int CalculationStar(int minCombo, int maxNodeFalse, int maxDrawingTime)
    {
        star = 0;
        if (comboCnt <= minCombo) ++star;
        if (nodeFalseCnt <= maxNodeFalse) ++star;
        if (drawingTime <= maxDrawingTime) ++star;

        return star;
    }

    public int CalculationSpeedRuleStar(int min, int middle, int max) {
        star = 0;
        if (drawingTime <= min) ++star;
        if (drawingTime <= middle) ++star;
        if (drawingTime <= max) ++star;
        return star;
    }
    public int CalculationPerfectRuleStar(int min, int middle, int max) {
        star = 0;
        if (comboCnt <= min) ++star;
        if (comboCnt <= middle) ++star;
        if (comboCnt <= max) ++star;
        return star;
    }
    public int CalculationOneTouchRuleStar(int min, int middle, int max) {
        star = 0;
        if (nodeFalseCnt >= min) ++star;
        if (nodeFalseCnt <= middle) ++star;
        if (nodeFalseCnt <= max) ++star;
        return star;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        EditorUtility.SetDirty(this);
    }



#endif
}
