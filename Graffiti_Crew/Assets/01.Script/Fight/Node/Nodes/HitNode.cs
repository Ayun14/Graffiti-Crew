using DG.Tweening;
using System;
using TMPro;
using UnityEngine;

public class HitNode : Node, INodeAction
{
    [SerializeField] private TextMeshProUGUI _hitCountText;

    private int _currentHitCount;

    private HitNodeDataSO _hitNodeData;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    public override void Init(StageGameRule stageGameRule, NodeJudgement judgement, NodeDataSO nodeData)
    {
        base.Init(stageGameRule, judgement, nodeData);

        _hitNodeData = nodeData as HitNodeDataSO;
        _renderer.sprite = _hitNodeData.sprite;
        transform.position = _hitNodeData.pos;
        lastNodePos = transform.position;

        NodeReset();
    }

    public override void SetAlpha(float endValue, float time = 0, Action callback = null)
    {
        _renderer.DOComplete();

        float startValue = endValue == 1f ? 0f : 1f;
        Color color = _renderer.color;
        color.a = startValue;
        _renderer.color = color;
        _renderer.DOFade(endValue, fadeTime)
            .OnComplete(() =>
            {
                callback?.Invoke();
            });
    }

    public void NodeStartAction()
    {
        if (isClearNode) return;

        // Particle
        PopGraffitiParticle(transform.position);

        // Sound
        GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Spray_Short);

        SetHitCount();
    }

    private void SetHitCount()
    {
        _stageGameRule.AddShakeSliderAmount(-_hitNodeData.sprayUseAmount);
        _stageGameRule.AddSpraySliderAmount(-_hitNodeData.sprayUseAmount);

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

        if (isClearNode == true) return;
        isClearNode = true;

        SetAlpha(0f, fadeTime, () => pool.Push(this));
    }

    public override void NodeFalse()
    {
        _judgement.NodeFalse();
        NodeReset();
    }

    public override void NodeReset()
    {
        _currentHitCount = _hitNodeData.hitNum;
        _hitCountText.text = _currentHitCount.ToString();
    }

    public override NodeType GetNodeType() => _hitNodeData.nodeType;

    public override NodeDataSO GetNodeDataSO() => _hitNodeData;
}
