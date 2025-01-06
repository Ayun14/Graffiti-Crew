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
    public Sprite nodeSprite;

    [Header("Curve")]
    [Range(2, 100)] public int points; // ���� ����
    [Range(-30, 30)] public float amplitude; // ���� (y�� ����)
}
