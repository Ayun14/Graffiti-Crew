using DG.Tweening;
using TMPro;
using UnityEngine;

public class HitNode : Node, INodeAction
{
    [SerializeField] private TextMeshProUGUI _hitCountText;
    [SerializeField] private float _fadeTime = 0.5f;

    private int _currentHitCount;

    private HitNodeDataSO _hitNodeData;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    public override void Init(NodeJudgement judgement, NodeDataSO nodeData)
    {
        base.Init(judgement, nodeData);

        _hitNodeData = nodeData as HitNodeDataSO;
        _renderer.sprite = _hitNodeData.sprite;
        transform.position = _hitNodeData.pos;

        _currentHitCount = _hitNodeData.hitNum;
        _hitCountText.text = _currentHitCount.ToString();

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
                    pool.Push(this); // Push
            });
    }

    public void NodeStartAction()
    {
        if (isClearNode) return;

        SetHitCount();
    }

    public void SetHitCount()
    {
        judgement.AddShakeSliderAmount(-_hitNodeData.sprayUseAmount);
        judgement.AddSpraySliderAmount(-_hitNodeData.sprayUseAmount);

        if (--_currentHitCount <= 0)
        {
            _hitCountText.text = string.Empty;
            NodeClear();
            return;
        }

        SetHitCountText(_currentHitCount);
    }

    private void SetHitCountText(int hitCount) => _hitCountText.text = hitCount.ToString();

    public override void NodeClear()
    {
        base.NodeClear();

        SetAlpha(0f);

        // 클리어 파티클?
    }

    public override NodeType GetNodeType()
    {
        return _hitNodeData.nodeType;
    }

    public override NodeDataSO GetNodeDataSO()
    {
        return _hitNodeData;
    }
}
