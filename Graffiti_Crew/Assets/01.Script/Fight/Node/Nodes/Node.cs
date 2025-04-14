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

    public virtual void Init(StageGameRule stageGameRule, NodeJudgement judgement, NodeDataSO nodeData)
    {
        if (_judgement == null)
            _judgement = judgement;

        if (_stageGameRule == null)
            _stageGameRule = stageGameRule;

        isClearNode = false;
        visibleTime = nodeData.visibleTime;

        if (stageGameRule.stageRule != StageRuleType.PerfectRule)
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

    protected void PopGraffitiParticle(Vector3 spawnPos)
    {
        IPoolable poolable = poolManagerSO.Pop(_graffitiParticleTypeSO);
        poolable.GameObject.transform.position = spawnPos;
        if (poolable.GameObject.transform.TryGetComponent(out GraffitiParticle graffitiParticle))
            graffitiParticle.ParticlePlay();
    }

    public void StartVisibleRoutine() => StartCoroutine("VisibleRoutine");
    public void StopVisibleRoutine() => StopCoroutine("VisibleRoutine");

    private IEnumerator VisibleRoutine()
    {
        SetAlpha(1, fadeTime);
        yield return new WaitForSeconds(fadeTime + visibleTime);
        SetAlpha(0, fadeTime, () => PushObj());
    }

    #region Pool
    public void SetUpPool(Pool pool)
    {
        this.pool = pool;
    }

    public void PushObj() => pool.Push(this);

    public void ResetItem() { }
    #endregion
}
