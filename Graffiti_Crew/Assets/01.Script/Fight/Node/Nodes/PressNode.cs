using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class PressNode : Node, INodeAction
{
    [Header("PressNode")]
    [SerializeField] private Color _fitColor;

    private float _currentTime = 0;
    private float _pressTime;

    private bool _isPressing;

    private PressNodeDataSO _pressNodeData;
    private SpriteRenderer _renderer;

    // Ring
    private SpriteRenderer _ringRenderer;
    private Vector3 _orginRingScale;
    private Vector3 _targetRingScale = new Vector3(0.1f, 0.1f, 1f);

    // Sound
    private SoundObject _sprayLongSoundObj;

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

        _pressTime = _pressNodeData.pressTime;
        _currentTime = 0;
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
        StopCoroutine(RingColorRoutine());
        StartCoroutine(RingColorRoutine());

        // Sound
        _sprayLongSoundObj = GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.Spray_Long, true)
            .GetComponent<SoundObject>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (_isPressing)
            {
                // 노드 성공
                CheckNodeClear();
            }
        }

        if (_isPressing)
        {
            // Particle
            PopGraffitiParticle(transform.position);

            _currentTime += Time.deltaTime;
        }
    }

    #region Clera Check

    private void CheckNodeClear()
    {
        if (Mathf.Abs(_pressTime - _currentTime) <= _pressNodeData.possibleRange)
        {
            NodeClear();
        }
        else
        {
            // 노드 실패 (중도 포기 실패)
            _stageGameRule.NodeFalse(this);
            ResetNode();

            // Sound
            _sprayLongSoundObj?.PushObject(true);
        }
    }

    public override void NodeClear()
    {
        base.NodeClear();

        if (isClearNode == true) return;
        isClearNode = true;

        // Combo
        _stageGameRule.NodeSuccess(this);

        SetAlpha(0f, fadeTime, () => pool.Push(this));
    }

    private void ResetNode()
    {
        _isPressing = false;
        SetRingScale(_orginRingScale, 0);
        _currentTime = 0; 

        // Sound
        _sprayLongSoundObj?.PushObject(true);
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
        yield return new WaitForSeconds(_pressTime - _pressNodeData.possibleRange);
        _ringRenderer.color = _fitColor;
        yield return new WaitForSeconds(_pressNodeData.possibleRange * 2);
        _ringRenderer.color = originColor;
    }

    #endregion

    public override NodeDataSO GetNodeDataSO() => _pressNodeData;

    public override NodeType GetNodeType() => _pressNodeData.nodeType;
}
