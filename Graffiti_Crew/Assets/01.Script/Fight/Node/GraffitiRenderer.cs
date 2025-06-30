using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraffitiRenderer : MonoBehaviour
{
    [Header("Pool")]
    [SerializeField] private PoolManagerSO _poolManager;
    [SerializeField] private PoolTypeSO _graffitiSuccessEffect;

    [Header("Renderer")]
    [SerializeField] private GameObject _graffitiRender;

    // Member Graffiti
    [SerializeField] private float _memberGraffitiTimeDuration = 4f;
    private SpriteRenderer _memberGraffitiRenderer;
    private Material _memberGraffitiMat;
    private ParticleSystem _memberGraffitiPS;
    private Vector3 _startPos;
    private Vector3 _endPos;

    protected StageGameRule _stageGameRule;

    private int _currentLayer = 0;

    private void Awake()
    {
        _memberGraffitiRenderer = GetComponent<SpriteRenderer>();
        _memberGraffitiMat = _memberGraffitiRenderer.material;
        _memberGraffitiPS = GetComponentInChildren<ParticleSystem>();

        SetMemberGraffitiMat(0f);
    }

    public void Init(StageGameRule stageGameRule, Sprite startSprite)
    {
        _stageGameRule = stageGameRule;

        _currentLayer = -stageGameRule.NodeCnt;
        _memberGraffitiRenderer.sortingOrder = _currentLayer;

        if (_memberGraffitiRenderer == null) return;
        _memberGraffitiRenderer.sprite = startSprite;

        Bounds bounds = _memberGraffitiRenderer.bounds;
        _startPos = new Vector3(bounds.min.x, bounds.center.y, -0.2f);
        _endPos = new Vector3(bounds.max.x, bounds.center.y, -0.2f);
    }

    public void SetSprite(NodeDataSO nodeData)
    {
        if (nodeData != null && nodeData.graffitiSprite == null) return;

        SpriteRenderer renderer = Instantiate(_graffitiRender, transform.position, Quaternion.identity, transform)
            .GetComponent<SpriteRenderer>();

        renderer.sprite = nodeData.graffitiSprite;
        renderer.sortingOrder = ++_currentLayer;
        PlayGraffitiSuccessEffect(nodeData);

        // Fade
        Color color = renderer.color;
        color.a = 0;
        renderer.color = color;
        renderer.DOFade(1, 0.6f);
    }

    private void PlayGraffitiSuccessEffect(NodeDataSO nodeData)
    {
        Vector3 spawnPos = Vector3.zero;
        switch (nodeData.nodeType)
        {
            case NodeType.SingleNode:
                SingleNodeDataSO singleNodeData = nodeData as SingleNodeDataSO;
                spawnPos = singleNodeData.pos;
                break;
            case NodeType.HitNode:
                HitNodeDataSO hitNodeDataSO = nodeData as HitNodeDataSO;
                spawnPos = hitNodeDataSO.pos;
                break;
            case NodeType.LongNode:
                LongNodeDataSO longNodeDataSO = nodeData as LongNodeDataSO;
                spawnPos = GetCenterPosition(longNodeDataSO.pointList);
                break;
            case NodeType.PressNode:
                PressNodeDataSO pressNodeData = nodeData as PressNodeDataSO;
                spawnPos = pressNodeData.pos;
                break;
        }
        GameObject effect = _poolManager.Pop(_graffitiSuccessEffect).GameObject;
        effect.transform.position = spawnPos;
    }

    private Vector3 GetCenterPosition(List<Vector3> positionList)
    {
        if (positionList == null || positionList.Count == 0)
            return Vector3.zero;

        Vector3 sum = Vector3.zero;
        foreach (Vector3 pos in positionList)
            sum += pos;

        return sum / positionList.Count;
    }

    public void ShowMemberGraffti()
    {
        SetMemberGraffitiMat(1, _memberGraffitiTimeDuration);
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
        _memberGraffitiMat.SetFloat("_RevealAmount", target == 0f ? 1f : 0f);
        _memberGraffitiPS.transform.position = _startPos;
        _memberGraffitiPS.Play();
        GameManager.Instance.SoundSystemCompo.PlayBGM(SoundType.Spray_Long);

        float currentTime = 0;
        while (currentTime < time)
        {
            currentTime += Time.deltaTime;
            float t = Mathf.Clamp01(currentTime / time);
            _memberGraffitiMat.SetFloat("_RevealAmount", t);
            _memberGraffitiPS.transform.position = Vector3.Lerp(_startPos, _endPos, t);
            yield return null;
        }

        _memberGraffitiMat.SetFloat("_RevealAmount", target);
        _memberGraffitiPS.Stop();
        GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Spray_Long);
    }
}
