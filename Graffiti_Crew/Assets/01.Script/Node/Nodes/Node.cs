using UnityEngine;

public abstract class Node : MonoBehaviour, IPoolable
{
    protected bool _isInitEnd = false;
    protected bool _isClearNode = false;

    protected NodeJudgement _judgement;

    [Header("Pool")]
    protected Pool _pool;

    [SerializeField] protected PoolTypeSO _poolType;
    public PoolTypeSO PoolType => _poolType;
    public GameObject GameObject => gameObject;

    public virtual void Init(NodeJudgement judgement, NodeDataSO nodeData)
    {
        if (_judgement == null)
            _judgement = judgement;

        _isInitEnd = false;
        _isClearNode = false;
    }

    public virtual void NodeClear()
    {
        if (_isClearNode == true) return;

        _isClearNode = true;

        // �ڽ��� Ŭ����� ����� Judgement���� �˸�
        _judgement?.CheckNodeClear(this);
    }

    public abstract NodeType GetNodeType();

    public void SetUpPool(Pool pool)
    {
        _pool = pool;
    }

    public void ResetItem()
    {

    }
}
