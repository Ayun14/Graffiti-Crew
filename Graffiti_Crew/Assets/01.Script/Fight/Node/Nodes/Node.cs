using System;
using System.Collections;
using UnityEngine;

public abstract class Node : MonoBehaviour, IPoolable
{
    protected bool isClearNode = false;

    protected StageGameRule _stageGameRule;
    protected NodeJudgement _judgement;

    [Header("Pool")]
    protected Pool pool;
    [SerializeField] protected PoolManagerSO poolManagerSO;
    [SerializeField] protected PoolTypeSO poolType;
    public PoolTypeSO PoolType => poolType;
    public GameObject GameObject => gameObject;

    [Header("Graffiti Particle")]
    [SerializeField] private PoolTypeSO _graffitiParticleTypeSO;

    [Header("Node")]
    public float fadeTime = 0.5f;
    [HideInInspector] public float visibleTime;
    private Coroutine _visibleRoutine;

    [HideInInspector] public Vector3 lastNodePos;

    public virtual void Init(StageGameRule stageGameRule, NodeJudgement judgement, NodeDataSO nodeData)
    {
        if (_judgement == null)
            _judgement = judgement;

        if (_stageGameRule == null)
            _stageGameRule = stageGameRule;

        isClearNode = false;
        visibleTime = nodeData.visibleTime;

        if (stageGameRule.stageRuleType != StageRuleType.PerfectRule)
            SetAlpha(1f, fadeTime);

    }

    public virtual void NodeClear()
    {
        if (isClearNode == true) return;

        // 자신이 클리어된 사실을 Judgement에게 알림
        _judgement?.NodeClear(this);
    }

    public abstract NodeType GetNodeType();

    public abstract NodeDataSO GetNodeDataSO();

    public abstract void SetAlpha(float endValue, float time = 0, Action callback = null);

    public abstract void NodeFalse();

    public abstract void NodeReset();

    protected void PopGraffitiParticle(Vector3 spawnPos)
    {
        IPoolable poolable = poolManagerSO.Pop(_graffitiParticleTypeSO);
        poolable.GameObject.transform.position = spawnPos;
        if (poolable.GameObject.transform.TryGetComponent(out GraffitiParticle graffitiParticle))
            graffitiParticle.ParticlePlay(GetNodeDataSO().particleColor);
    }

    public void StartVisibleRoutine(Action onComplete = null)
    {
        if (_visibleRoutine != null) StopCoroutine(_visibleRoutine);
        _visibleRoutine = StartCoroutine(VisibleRoutine(onComplete));
    }

    private IEnumerator VisibleRoutine(Action onComplete = null)
    {
        bool fadedOut = false;
        SetAlpha(1, fadeTime, () => fadedOut = true);

        yield return new WaitUntil(() => fadedOut);
        yield return new WaitForSeconds(visibleTime);

        fadedOut = false;
        SetAlpha(0, fadeTime, () =>
        {
            onComplete?.Invoke();
            PushObj();
        });
    }

    #region Pool
    public void SetUpPool(Pool pool)
    {
        this.pool = pool;
    }

    public void PushObj()
    {
        if (_visibleRoutine != null) StopCoroutine(_visibleRoutine);
        pool.Push(this);
    }

    public void ResetItem() { }
    #endregion
}
