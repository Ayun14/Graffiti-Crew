using DG.Tweening;
using UnityEngine;

public class PressNode : Node, INodeAction
{
    [SerializeField] private float _fadeTime = 0.5f;

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

    public override void Init(NodeJudgement judgement, NodeDataSO nodeData)
    {
        base.Init(judgement, nodeData);

        _pressNodeData = nodeData as PressNodeDataSO;
        _renderer.sprite = _pressNodeData.sprite;
        transform.position = _pressNodeData.pos;

        _pressTime = _pressNodeData.pressTime;
        _currentTime = 0;

        SetAlpha(1f);
    }

    private void SetAlpha(float endValue)
    {
        // Init
        float startValue = endValue == 1f ? 0f : 1f;
        Color color = _renderer.color;
        color.a = startValue;
        _renderer.color = color;
        _ringRenderer.color = color;

        _ringRenderer.DOFade(endValue, _fadeTime);
        _renderer.DOFade(endValue, _fadeTime)
            .OnComplete(() =>
            {
                if (endValue == 0f)
                {
                    ResetNode();
                    pool.Push(this); // Push
                }
            });
    }

    public void NodeStartAction()
    {
        if (isClearNode) return;

        _isPressing = true;
        SetRingScale(_targetRingScale, _pressTime);

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
            // Combo
            judgement.NodeSuccess(this);

            // Particle
            PopGraffitiParticle(transform.position);

            _currentTime += Time.deltaTime;
        }
    }

    #region Clera Check

    private void CheckNodeClear()
    {
        if (_pressTime - _currentTime <= 0.5f)
        {
            NodeClear();
        }
        else
        {
            // 노드 실패 (중도 포기 실패)
            judgement.NodeFalse(this);
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

        SetAlpha(0f);
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

    #endregion

    public override NodeDataSO GetNodeDataSO() => _pressNodeData;

    public override NodeType GetNodeType() => _pressNodeData.nodeType;
}
