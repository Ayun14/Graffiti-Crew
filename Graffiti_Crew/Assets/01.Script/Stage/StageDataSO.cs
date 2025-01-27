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
    public int clearTime; // �� ����
    public GameObject rivalPrefab;

    [Header("Map")]
    public GameObject mapPrefab;

    [Header("Dialogue")]
    public GameObject �̰�����;
    // ������ ���⿡ �Ұ� �߰���

    [Header("Bool")]
    public bool isClearStage = true;
}
