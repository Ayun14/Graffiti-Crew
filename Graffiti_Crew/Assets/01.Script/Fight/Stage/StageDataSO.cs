using AH.SaveSystem;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDataSO", menuName = "SO/Stage/StageDataSO")]
public class StageDataSO : ScriptableObject
{
    [Header("Player Graffiti")]
    public Sprite startGraffiti;
    public List<NodeDataSO> nodeDatas;

    [Header("Rival")]
    public Sprite rivalGraffiti;
    public int rivalClearTime; // √  ¥‹¿ß
    public Transform rivalPrefab;

    [Header("Map")]
    public Transform mapPrefab;

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
