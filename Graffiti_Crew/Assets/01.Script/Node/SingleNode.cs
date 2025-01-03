using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SingleNode : Node
{
    [SerializeField] private SingleNodeDataSO _singleNodeData;

    private SpriteRenderer _renderer;

    public override void Init()
    {
        base.Init();
        _renderer = GetComponentInChildren<SpriteRenderer>();

        _renderer.sprite = _singleNodeData.sprite;
        transform.position = _singleNodeData.pos;
    }

    public override NodeType GetNodeType()
    {
        return _singleNodeData.nodeType;
    }
}
