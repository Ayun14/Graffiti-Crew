using System.Collections.Generic;
using UnityEngine;

public class NodeSpawner : MonoBehaviour
{
    [SerializeField] private PoolManagerSO _poolManager;
    [SerializeField] private List<PoolTypeSO> _poolTypes = new();

    private Queue<NodeDataSO> _nodeDatas = new();

    private NodeJudgement _judgement;
    private Node _currentNode;

    public void Init(NodeJudgement judgement, List<NodeDataSO> nodeDatas)
    {
        _judgement = judgement;
        _judgement.OnNodeSpawnStart += HandleNodeSpawn;

        _currentNode = null;

        ResetSpawner();
        foreach (NodeDataSO data in nodeDatas)
            _nodeDatas.Enqueue(data);
    }

    private void OnDestroy()
    {
        _judgement.OnNodeSpawnStart -= HandleNodeSpawn;
    }

    private void HandleNodeSpawn()
    {
        NodeSpawn();
    }

    private void NodeSpawn()
    {
        if (_nodeDatas == null) return;
        if (_nodeDatas.Count == 0)
        {
            _judgement.AllNodeClear();
        }
        else
        {
            // PoolManager에서 가져오기
            NodeDataSO nodeData = _nodeDatas.Peek();
            PoolTypeSO poolType = _poolTypes.Find(type => type.name == nodeData.nodeType.ToString());
            IPoolable poolGo = _poolManager.Pop(poolType);
            _currentNode = poolGo.GameObject.GetComponent<Node>();

            if (poolGo.GameObject != null && poolGo.GameObject.TryGetComponent(out Node node))
                node.Init(_judgement, nodeData);

            _nodeDatas.Dequeue();
        }
    }

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
