using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public enum LongNodeType
{
    // ����, �
    Stright, Curve
}

[CreateAssetMenu(fileName = "LongNodeDataSO", menuName = "SO/Lode/LongNodeDataSO")]
public class LongNodeDataSO : NodeDataSO
{
    [Header("Data")]
    public LongNodeType longNodeType;
    public Material lineRendererMat;
    public List<Vector3> pointList;

    [Header("Start And End")]
    public Sprite startNodeSprite;
    public Sprite endNodeSprite;

    [Header("Curve")]
    [Range(2, 100)] public int points; // ���� ����
    [Range(-30, 30)] public float amplitude; // ���� (y�� ����)




    public float followThreshold = 0.5f; // ��� ���� ��� ����
    public float failThreshold = 1.5f;   // ��� ��Ż ��� ����
    public float startThreshold = 0.5f; // ������ ���� ��� ����
}
