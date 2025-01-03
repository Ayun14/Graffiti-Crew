using UnityEngine;

public class HitNode : Node
{
    [SerializeField] private HitNodeDataSO _hitNodeData;

    private SpriteRenderer _renderer;

    public override void Init()
    {
        base.Init();
        _renderer = GetComponentInChildren<SpriteRenderer>();

        _renderer.sprite = _hitNodeData.sprite;
        transform.position = _hitNodeData.pos;
    }

    public override NodeType GetNodeType()
    {
        return _hitNodeData.nodeType;
    }
}
