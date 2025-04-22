using System.Collections.Generic;
using UnityEngine;

public abstract class NodeSpawner : MonoBehaviour
{
    [SerializeField] protected PoolManagerSO _poolManager;
    [SerializeField] protected List<PoolTypeSO> _poolTypes = new();

    protected Queue<NodeDataSO> _nodeDatas = new();

    protected StageGameRule _stageGameRule;
    protected NodeJudgement _nodeJudgement;
    protected Node _currentNode;

    public void Init(StageGameRule stageGameRule, NodeJudgement nodeJudgement, List<NodeDataSO> nodeDatas)
    {
        _stageGameRule = stageGameRule;
        _nodeJudgement = nodeJudgement;

        _currentNode = null;

        ResetSpawner();
        foreach (NodeDataSO data in nodeDatas)
            _nodeDatas.Enqueue(data);
    }

    public abstract void NodeSpawn();

    public void DequeueNode()
    {
        _nodeDatas.Dequeue();

        if (_nodeDatas.Count == 0) _stageGameRule.AllNodeClear();
    }

    public NodeDataSO PeekNode() => _nodeDatas.Peek();

    public void ResetSpawner()
    {
        _nodeDatas.Clear();
    }

    public void StopSpawn()
    {
        if (_currentNode != null)
            _currentNode.PushObj();
    }
}
