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

        // Ŭ���� ��ƼŬ?
        // Ǯ�Ŵ����� ����ֱ�
    }

    public override NodeType GetNodeType()
    {
        return _singleNodeData.nodeType;
    }
}
