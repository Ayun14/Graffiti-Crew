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
    [Header("-------All-------")]
    [Header("Stage")]
    public StageType stageType;
    public StageRuleType stageRuleType;
    public GameObject judgementPrefab;
    public GameObject spawnerPrefab;

    [Header("Environment")]
    public GameObject mapPrefab;
    public Material skyBoxMat;
    public GameObject weatherParticle;

    [Header("Next Stage")]
    public StageType nextStagetype;
    public string nextChapter;
    public string nextStage;

    [Header("-------Story Stage-------")]
    [Header("Dialogue")]
    public DialogueDataReader dialogueData_KR;
    public DialogueDataReader dialogueData_EN;
    public StoryDialogueSO storyDialogue;

    [Header("-------Play Stage-------")]
    [Header("Player Graffiti")]
    public Sprite memberGraffiti;
    public List<NodeDataSO> nodeDatas;

    [Header("Rival Prefab")]
    public List<Transform> rivalPrefabList;

    [Header("-------Activity Stage-------")]
    public bool isOnPolice;

    [Header("-------Fight Stage-------")]
    [Header("Rival Graffiti")]
    public List<Sprite> rivalGraffiti;
    public int rivalClearTime; // �� ����

    [Header("Stage Star Setting")]
    public StageResultSO stageResult;
    public StageSaveDataSO stageSaveData;

    public int minStandard;
    public int middleStandard;
    public int maxStandard;

    [Header("Bool")]
    public bool isPlayerWin = false;

#if UNITY_EDITOR
    private void OnValidate()
    {
        EditorUtility.SetDirty(this);
    }
#endif
}
