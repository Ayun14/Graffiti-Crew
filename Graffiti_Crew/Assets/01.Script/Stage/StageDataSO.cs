using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageDataSO", menuName = "SO/Stage/StageDataSO")]
public class StageDataSO : ScriptableObject
{
    [Header("Player Graffiti")]
    public Sprite startGraffiti;
    public List<NodeDataSO> nodeDatas;

    [Header("Rival")]
    public Sprite rivalGraffiti;
    public int clearTime; // 초 단위
    public GameObject rivalPrefab;

    [Header("Map")]
    public GameObject mapPrefab;

    [Header("Dialogue")]
    public GameObject 이건지워;
    // 공서연 여기에 할거 추가해

    [Header("Bool")]
    public bool isClearStage = true;
}
