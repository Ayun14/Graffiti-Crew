using UnityEngine;

public class GraffitiRenderer : MonoBehaviour
{
    private SpriteRenderer _renderer;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void SetStartSprite(Sprite startSprite)
    {
        _renderer.sprite = startSprite;
    }

    public void SetSprite(Sprite sprite)
    {
        if (_renderer == null || sprite == null) return;

        _renderer.sprite = sprite;
    }
}
