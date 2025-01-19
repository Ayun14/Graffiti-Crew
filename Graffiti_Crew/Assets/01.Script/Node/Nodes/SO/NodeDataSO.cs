using UnityEngine;

public enum NodeType
{
    // ¥‹¿œ, ø¨≈∏, ∑’
    SingleNode, HitNode, LongNode
}

public class NodeDataSO : ScriptableObject
{
    [Header("Node Data")]
    public NodeType nodeType;
    public Sprite graffitiSprite;
}
