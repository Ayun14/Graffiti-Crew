using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    // ����, ��, ��Ÿ
    SingleNode, LongNode, HitNode
}

public class NodeDataSO : ScriptableObject
{
    public NodeType nodeType;
}
