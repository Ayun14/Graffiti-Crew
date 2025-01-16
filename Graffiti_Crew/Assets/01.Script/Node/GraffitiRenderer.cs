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

        _renderer.sprite = null;
    }

    public void SetSprite(Sprite sprite)
    {
        if (_renderer == null || sprite == null) return;

        _renderer.sprite = sprite;
    }
}
