using UnityEngine;

public class GraffitiRenderer : MonoBehaviour
{
    private SpriteRenderer _renderer;

    private NodeJudgement _judgement;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void Init(NodeJudgement judgement, Sprite startSprite)
    {
        _judgement = judgement;

        if (_renderer == null) return;
        _renderer.sprite = startSprite;
    }

    public void SetSprite(Sprite sprite)
    {
        if (_renderer == null || sprite == null) return;

        _renderer.sprite = sprite;
    }
}
