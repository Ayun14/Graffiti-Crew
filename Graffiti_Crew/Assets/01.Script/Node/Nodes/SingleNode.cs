using DG.Tweening;
using UnityEngine;

public class SingleNode : Node
{
    [SerializeField] private SingleNodeDataSO _singleNodeData;
    [SerializeField] private float _fadeTime = 1f;

    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();
    }

    public override void Init()
    {
        base.Init();

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
                    gameObject.SetActive(false); // Push
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
}
