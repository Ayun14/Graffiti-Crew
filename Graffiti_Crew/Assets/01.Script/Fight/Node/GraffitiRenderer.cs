using DG.Tweening;
using NUnit.Framework;
using System.Collections;
using UnityEngine;

public class GraffitiRenderer : MonoBehaviour
{
    [SerializeField] private GameObject _graffitiRender;

    protected StageGameRule _stageGameRule;
    private SpriteRenderer _memberGraffitiRenderer;
    private Material _memberGraffitiMat;

    private int _currentLayer = 0;

    private void Awake()
    {
        _memberGraffitiRenderer = GetComponent<SpriteRenderer>();
        _memberGraffitiMat = _memberGraffitiRenderer.material;
        SetMemberGraffitiMat(0f);
    }

    public void Init(StageGameRule stageGameRule, Sprite startSprite)
    {
        _stageGameRule = stageGameRule;

        _currentLayer = -stageGameRule.NodeCnt;
        _memberGraffitiRenderer.sortingOrder = _currentLayer;

        if (_memberGraffitiRenderer == null) return;
        _memberGraffitiRenderer.sprite = startSprite;
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

    public void ShowMemberGraffti()
    {
        SetMemberGraffitiMat(1, 6f);
    }

    private void SetMemberGraffitiMat(float target, float time = 0)
    {
        if (time == 0) 
            _memberGraffitiMat.SetFloat("_RevealAmount", target);
        else 
            StartCoroutine(SetMemberGraffitiRoutine(target, time));
    }

    private IEnumerator SetMemberGraffitiRoutine(float target, float time)
    {
        float currentValue = target == 0f ? 1f : 0f;
        while (currentValue < target)
        {
            currentValue += Time.deltaTime;
            float t = Mathf.Clamp01(currentValue / target);
            _memberGraffitiMat.SetFloat("_RevealAmount", t);
            yield return null;
        }
        _memberGraffitiMat.SetFloat("_RevealAmount", target);
    }
}
