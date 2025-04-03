using AH.SaveSystem;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum StageRuleType
{
    SpeedRule, PerfectRule, OneTouchRule
}

[CreateAssetMenu(fileName = "StageDataSO", menuName = "SO/Stage/StageDataSO")]
public class StageDataSO : ScriptableObject
{
    [Header("Stage")]
    public StageRuleType stageRule;
    public Transform mapPrefab;

    [Header("Player Graffiti")]
    public Sprite startGraffiti;
    public List<NodeDataSO> nodeDatas;

    [Header("Rival")]
    public Sprite rivalGraffiti;
    public int rivalClearTime; // √  ¥‹¿ß
    public Transform rivalPrefab;

    [Header("Dialogue")]
    public DialogueDataReader dialogueData_KR;
    public DialogueDataReader dialogueData_EN;
    public StoryDialogueSO storyDialogue;

    [Header("Stage Star Setting")]
    public StageResultSO stageResult;
    public StageSaveDataSO stageSaveData;
    public int minCombo;
    public int maxNodeFalse;
    public int maxDrawingTime;

    [Header("Bool")]
    public bool isClearStage = false;

#if UNITY_EDITOR
    private void OnValidate()
    {
        EditorUtility.SetDirty(this);
    }
#endif
}
