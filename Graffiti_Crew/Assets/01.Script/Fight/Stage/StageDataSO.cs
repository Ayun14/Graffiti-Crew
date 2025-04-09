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
    public StageType stagetype;
    public StageRuleType stageRuleType;
    public GameObject judgementPrefab;
    public GameObject spawnerPrefab;
    public GameObject mapPrefab;

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

    public int minStandard;
    public int middleStandard;
    public int maxStandard;

    [Header("Bool")]
    public bool isClearStage = false;

#if UNITY_EDITOR
    private void OnValidate()
    {
        EditorUtility.SetDirty(this);
    }
#endif
}
