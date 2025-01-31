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
    public int clearTime; // √  ¥‹¿ß
    public GameObject rivalPrefab;

    [Header("Map")]
    public GameObject mapPrefab;

    [Header("Dialogue")]
    public DialogueDataReader dialogueData;

    [Header("Stage Star Setting")]
    public StageResultSO stageResult;
    public int minCombo;
    public int maxNodeFalse;
    public int mindrawingTime;

    [Header("Bool")]
    public bool isClearStage = false;

#if UNITY_EDITOR
    private void OnValidate()
    {
        EditorUtility.SetDirty(this);
    }
#endif
}
