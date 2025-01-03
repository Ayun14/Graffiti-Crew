using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    // ¥‹¿œ, ∑’, ø¨≈∏
    SingleNode, LongNode, HitNode
}

public class NodeDataSO : ScriptableObject
{
    public NodeType nodeType;
}
