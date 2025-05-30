using DG.Tweening;
using NUnit.Framework;
using UnityEngine;

public class GraffitiRenderer : MonoBehaviour
{
    [SerializeField] private GameObject _graffitiRender;

    private SpriteRenderer _renderer;
    protected StageGameRule _stageGameRule;

    private int _currentLayer = 0;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void Init(StageGameRule stageGameRule, Sprite startSprite)
    {
        _stageGameRule = stageGameRule;

        _currentLayer = -stageGameRule.NodeCnt;
        _renderer.sortingOrder = _currentLayer;

        if (_renderer == null) return;
        _renderer.sprite = startSprite;
    }

    public void SetSprite(Sprite sprite)
    {
        if (sprite == null) return;

        SpriteRenderer renderer = Instantiate(_graffitiRender, transform.position, Quaternion.identity, transform)
            .GetComponent<SpriteRenderer>();

        renderer.sprite = sprite;
        renderer.sortingOrder = ++_currentLayer;

        // Fade
        Color color = renderer.color;
        color.a = 0;
        renderer.color = color;
        renderer.DOFade(1, 0.6f);
    }
}
