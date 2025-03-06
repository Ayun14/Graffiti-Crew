using UnityEngine;

public abstract class Node : MonoBehaviour, IPoolable
{
    protected bool isClearNode = false;

    protected NodeJudgement judgement;

    [Header("Pool")]
    protected Pool pool;
    [SerializeField] protected PoolTypeSO poolType;
    public PoolTypeSO PoolType => poolType;
    public GameObject GameObject => gameObject;

    public virtual void Init(NodeJudgement judgement, NodeDataSO nodeData)
    {
        if (this.judgement == null)
            this.judgement = judgement;

        isClearNode = false;
    }

    public virtual void NodeClear()
    {
        if (isClearNode == true) return;

        // 자신이 클리어된 사실을 Judgement에게 알림
        judgement?.CheckNodeClear(this);
    }

    public abstract NodeType GetNodeType();

    public abstract NodeDataSO GetNodeDataSO();

    #region Pool
    public void SetUpPool(Pool pool)
    {
        this.pool = pool;
    }

    public void PushObj() => pool.Push(this);

    public void ResetItem() { }
    #endregion
}
