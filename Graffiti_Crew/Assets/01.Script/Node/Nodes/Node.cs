using UnityEngine;

public abstract class Node : MonoBehaviour, IPoolable
{
    protected bool isInitEnd = false;
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

        isInitEnd = false;
        isClearNode = false;
    }

    public virtual void NodeClear()
    {
        if (isClearNode == true) return;

        isClearNode = true;

        // �ڽ��� Ŭ����� ����� Judgement���� �˸�
        judgement?.CheckNodeClear(this);
    }

    public abstract NodeType GetNodeType();

    public abstract NodeDataSO GetNodeDataSO();

    #region Pool
    public void SetUpPool(Pool pool)
    {
        this.pool = pool;
    }

    public void ResetItem() { }
    #endregion
}
