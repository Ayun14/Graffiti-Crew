using UnityEngine;

public enum NodeType
{
    // 단일, 연타, 롱, 길게누르기, 부메랑
    SingleNode, HitNode, LongNode, PressNode, BoomerangNode
}

public class NodeDataSO : ScriptableObject
{
    [Header("Node Data")]
    public NodeType nodeType;
    public Sprite graffitiSprite;
    public float sprayUseAmount;
    public float visibleTime;
}
