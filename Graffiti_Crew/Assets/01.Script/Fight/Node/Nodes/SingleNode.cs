using DG.Tweening;
using System;
using UnityEngine;

public class SingleNode : Node, INodeAction
{
    private SingleNodeDataSO _singleNodeData;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    public override void Init(StageGameRule stageGameRule, NodeJudgement judgement, NodeDataSO nodeData)
    {
        base.Init(stageGameRule, judgement, nodeData);

        _singleNodeData = nodeData as SingleNodeDataSO;
        _renderer.sprite = _singleNodeData.sprite;
        transform.position = _singleNodeData.pos;
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

    public override void NodeClear()
    {
        base.NodeClear();

        if (isClearNode == true) return;
        isClearNode = true;

        // Combo
        _stageGameRule.NodeSuccess(this);

        // Particle
        PopGraffitiParticle(transform.position);

        // Sound
        GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Spray_Short);

        SetAlpha(0f, fadeTime, () => pool.Push(this));

        _stageGameRule.AddShakeSliderAmount(-_singleNodeData.sprayUseAmount);
        _stageGameRule.AddSpraySliderAmount(-_singleNodeData.sprayUseAmount);
    }

    public override NodeType GetNodeType()
    {
        return _singleNodeData.nodeType;
    }

    public override NodeDataSO GetNodeDataSO()
    {
        return _singleNodeData;
    }

    public void NodeStartAction()
    {
        NodeClear();
    }
}
