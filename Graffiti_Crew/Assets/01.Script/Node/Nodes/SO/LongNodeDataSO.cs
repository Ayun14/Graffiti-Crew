using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public enum LongNodeType
{
    // 직선, 곡선
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
    public Sprite nodeSprite;

    [Header("Curve")]
    [Range(2, 100)] public int points; // 점의 개수
    [Range(-30, 30)] public float amplitude; // 진폭 (y축 높이)
}
