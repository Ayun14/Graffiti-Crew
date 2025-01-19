using UnityEngine;

public enum NodeType
{
    // ����, ��Ÿ, ��
    SingleNode, HitNode, LongNode
}

public class NodeDataSO : ScriptableObject
{
    [Header("Node Data")]
    public NodeType nodeType;
    public Sprite graffitiSprite;
}
