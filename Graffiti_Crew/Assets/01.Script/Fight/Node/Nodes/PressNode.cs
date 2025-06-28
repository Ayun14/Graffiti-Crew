using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class PressNode : Node, INodeAction
{
    [Header("PressNode")]
    [SerializeField] private Color _fitColor;
    [SerializeField] private float _pressTime;
    [SerializeField] private float _possibleRange;
    private float _currentTime = 0;

    private bool _isPressing = false;

    private PressNodeDataSO _pressNodeData;
    private SpriteRenderer _renderer;

    // Particle
    private float _currentParticleSpawnTime = 0;
    private float _particleSpawnTime = 0.2f;

    // Ring
    private SpriteRenderer _ringRenderer;
    private Vector3 _orginRingScale;
    private Vector3 _targetRingScale = new Vector3(0.085f, 0.085f, 1f);
    private Coroutine _ringColorCoroutine;

    private void Awake()
    {
        _renderer = transform.Find("Visual").GetComponent<SpriteRenderer>();
        _ringRenderer = transform.Find("RingRenderer").GetComponent<SpriteRenderer>();
        _orginRingScale = _ringRenderer.transform.localScale;
    }

    public override void Init(StageGameRule stageGameRule, NodeJudgement judgement, NodeDataSO nodeData)
    {
        base.Init(stageGameRule, judgement, nodeData);

        _pressNodeData = nodeData as PressNodeDataSO;
        _renderer.sprite = _pressNodeData.sprite;
        transform.position = _pressNodeData.pos;
        lastNodePos = transform.position;

        NodeReset();
    }

    public override void SetAlpha(float endValue, float time = 0, Action callback = null)
    {
        _ringRenderer.DOComplete();
        _renderer.DOComplete();

        // Init
        float startValue = endValue == 1f ? 0f : 1f;
        Color color = _renderer.color;
        color.a = startValue;
        _renderer.color = color;
        _ringRenderer.color = color;

        _ringRenderer.DOFade(endValue, fadeTime);
        _renderer.DOFade(endValue, fadeTime)
            .OnComplete(() =>
            {
                callback?.Invoke();
            });
    }

    public void NodeStartAction()
    {
        if (isClearNode) return;

        _isPressing = true;

        // Ring
        SetRingScale(_targetRingScale, _pressTime);
        if (_ringColorCoroutine != null) StopCoroutine(_ringColorCoroutine);
        _ringColorCoroutine = StartCoroutine(RingColorRoutine());

        // Sound
        GameManager.Instance.SoundSystemCompo.PlayBGM(SoundType.Spray_Long);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (_isPressing) CheckNodeClear();
        }

        if (_isPressing)
        {
            _currentTime += Time.deltaTime;

            // Particle
            _currentParticleSpawnTime += Time.deltaTime;
            if(_currentParticleSpawnTime >= _particleSpawnTime)
            {
                PopGraffitiParticle(transform.position);
                _currentParticleSpawnTime = 0;
            }
        }
    }

    #region Clera Check

    private void CheckNodeClear()
    {
        if (Mathf.Abs(_pressTime - _currentTime) <= _possibleRange)
        {
            NodeClear();
        }
        else
        {
            // 노드 실패 (중도 포기 실패)
            NodeFalse();
        }
    }

    public override void NodeClear()
    {
        base.NodeClear();

        if (isClearNode == true) return;
        isClearNode = true;
        NodeReset();

        SetAlpha(0f, fadeTime, () => pool.Push(this));
    }

    public override void NodeFalse()
    {
        _judgement.NodeFalse();
        NodeReset();
    }

    public override void NodeReset()
    {
        _isPressing = false;
        _currentTime = 0;
        _currentParticleSpawnTime = 0;
        SetRingScale(_orginRingScale, 0);
        if (_ringColorCoroutine != null)
        {
            StopCoroutine(_ringColorCoroutine);
            _ringColorCoroutine = null;
        }

        // Sound
        GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Spray_Long);
    }

    #endregion

    #region Ring

    private void SetRingScale(Vector3 target, float time)
    {
        _ringRenderer.transform.DOKill();
        _ringRenderer.transform.DOScale(target, time);
    }

    private IEnumerator RingColorRoutine()
    {
        Color originColor = _ringRenderer.color;
        yield return new WaitForSeconds(_pressTime - _possibleRange);
        _ringRenderer.color = _fitColor;
        yield return new WaitForSeconds(_possibleRange * 2);
        _ringRenderer.color = originColor;
    }

    #endregion

    public override NodeDataSO GetNodeDataSO() => _pressNodeData;

    public override NodeType GetNodeType() => _pressNodeData.nodeType;
}
