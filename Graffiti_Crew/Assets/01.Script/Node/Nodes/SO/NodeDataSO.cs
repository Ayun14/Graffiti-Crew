using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    // ¥‹¿œ, ø¨≈∏, ∑’
    SingleNode, HitNode, LongNode
}

public class NodeDataSO : ScriptableObject
{
    public NodeType nodeType;
}
