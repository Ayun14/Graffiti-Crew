using UnityEngine;

public enum NodeType
{
    // ����, ��Ÿ, ��, ��Դ�����, �θ޶�
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
