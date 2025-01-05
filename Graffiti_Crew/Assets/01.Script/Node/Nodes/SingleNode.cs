using UnityEngine;

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

    public override void NodeClear()
    {
        base.NodeClear();

        // 클리어 파티클?
        // 풀매니저에 집어넣기
    }

    public override NodeType GetNodeType()
    {
        return _singleNodeData.nodeType;
    }
}
