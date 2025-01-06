using TMPro;
using UnityEngine;

public class HitNode : Node
{
    [SerializeField] private HitNodeDataSO _hitNodeData;
    [SerializeField] private TextMeshProUGUI _hitCountText;

    private int _currentHitCount;

    private SpriteRenderer _renderer;

    public override void Init()
    {
        base.Init();
        _renderer = GetComponentInChildren<SpriteRenderer>();

        _renderer.sprite = _hitNodeData.sprite;
        transform.position = _hitNodeData.pos;

        _currentHitCount = _hitNodeData.hitNum;
        _hitCountText.text = _currentHitCount.ToString();
    }

    public void SetHitCount()
    {
        if (--_currentHitCount <= 0)
        {
            NodeClear();
            return;
        }

        SetHitCountText(_currentHitCount);
    }

    private void SetHitCountText(int hitCount) => _hitCountText.text = hitCount.ToString();

    public override void NodeClear()
    {
        base.NodeClear();

        // 클리어 파티클?
        // 풀매니저에 집어넣기
    }

    public override NodeType GetNodeType()
    {
        return _hitNodeData.nodeType;
    }
}
