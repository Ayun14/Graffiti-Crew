using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "StageResultSO", menuName = "SO/Stage/StageResultSO")]
public class StageResultSO : ScriptableObject
{
    public int value;

    public void Reset()
    {
        value = 0;
    }

    public int CalculationStar(int minCombo, int maxNodeFalse, int maxDrawingTime) 
    {
        int star = 0;

        if (value <= minCombo) ++star;
        if (value <= maxNodeFalse) ++star;
        if (value <= maxDrawingTime) ++star;

        return star;
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        EditorUtility.SetDirty(this);
    }
#endif
}
