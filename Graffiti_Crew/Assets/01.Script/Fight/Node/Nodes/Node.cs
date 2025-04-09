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

    public virtual void Init(StageGameRule stageGameRule, NodeJudgement judgement, NodeDataSO nodeData)
    {
        if (_judgement == null)
            _judgement = judgement;

        if (_stageGameRule == null)
            _stageGameRule = stageGameRule;

        isClearNode = false;
    }

    public virtual void NodeClear()
    {
        if (isClearNode == true) return;

        // 자신이 클리어된 사실을 Judgement에게 알림
        _judgement?.NodeClear(this);
    }

    public abstract NodeType GetNodeType();

    public abstract NodeDataSO GetNodeDataSO();

    protected void PopGraffitiParticle(Vector3 spawnPos)
    {
        IPoolable poolable = poolManagerSO.Pop(_graffitiParticleTypeSO);
        poolable.GameObject.transform.position = spawnPos;
        if (poolable.GameObject.transform.TryGetComponent(out GraffitiParticle graffitiParticle))
            graffitiParticle.ParticlePlay();

        // Sound
        //SoundManager.Instance.PlaySound(SoundType.Spray);
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
