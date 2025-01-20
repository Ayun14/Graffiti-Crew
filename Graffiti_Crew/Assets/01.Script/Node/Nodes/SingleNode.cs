using DG.Tweening;
using UnityEngine;

public class SingleNode : Node, INodeAction
{
    [SerializeField] private float _fadeTime = 1f;

    private SingleNodeDataSO _singleNodeData;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    public override void Init(NodeJudgement judgement, NodeDataSO nodeData)
    {
        base.Init(judgement, nodeData);

        _singleNodeData = nodeData as SingleNodeDataSO;
        _renderer.sprite = _singleNodeData.sprite;
        transform.position = _singleNodeData.pos;

        SetAlpha(1f);
    }

    private void SetAlpha(float endValue)
    {
        float startValue = endValue == 1f ? 0f : 1f;
        Color color = _renderer.color;
        color.a = startValue;
        _renderer.color = color;
        _renderer.DOFade(endValue, _fadeTime)
            .OnComplete(() =>
            {
                if (endValue == 0f)
                    _pool.Push(this); // Push
            });
    }

    public override void NodeClear()
    {
        base.NodeClear();

        SetAlpha(0f);

        // 클리어 파티클?
    }

    public override NodeType GetNodeType()
    {
        return _singleNodeData.nodeType;
    }

    public override NodeDataSO GetNodeDataSO()
    {
        return _singleNodeData;
    }

    public override float GetSprayUseAmount()
    {
        return _singleNodeData.sprayUseAmount;
    }

    public void NodeStartAction()
    {
        NodeClear();
    }
}
