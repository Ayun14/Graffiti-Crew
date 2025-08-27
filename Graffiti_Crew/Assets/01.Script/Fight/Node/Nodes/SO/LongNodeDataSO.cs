using System.Collections.Generic;
using UnityEngine;

public enum LongNodeType
{
    // 직선, 곡선
    Stright, Curve
}

[CreateAssetMenu(fileName = "LongNodeDataSO", menuName = "SO/Node/LongNodeDataSO")]
public class LongNodeDataSO : NodeDataSO
{
    [Header("Long Node Data")]
    public LongNodeType longNodeType;
    public Material lineRendererMat;
    [Range(2, 100)] public int points; // 점의 개수
    public List<Vector3> pointList;

    [Header("Start And End")]
    public Sprite startNodeSprite;
    public Sprite endNodeSprite;

    [Header("Curve")]
    [Range(-30, 30)] public float amplitude; // 진폭 (y축 높이)
}
