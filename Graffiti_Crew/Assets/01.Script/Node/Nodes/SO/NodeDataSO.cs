using System.Collections.Generic;
using UnityEngine;

public enum NodeType
{
    // ����, ��Ÿ, ��
    SingleNode, HitNode, LongNode
}

public class NodeDataSO : ScriptableObject
{
    public NodeType nodeType;
}
