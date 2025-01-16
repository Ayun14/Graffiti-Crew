using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NodeSpawner : MonoBehaviour
{
    [SerializeField] private PoolManagerSO _poolManager;
    [SerializeField] private List<PoolTypeSO> _poolTypes = new();

    private Queue<NodeDataSO> _nodeDatas = new();

    private NodeJudgement _judgement;

    private void Awake()
    {
        _judgement = GetComponentInParent<NodeJudgement>();
    }

    private void OnEnable()
    {
        _judgement.OnNodeSpawnStart += HandleNodeSpawn;
    }

    private void OnDisable()
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
            Debug.Log("모든 노드 클리어");
        }
        else
        {
            // PoolManager에서 가져오기
            NodeDataSO nodeData = _nodeDatas.Peek();
            PoolTypeSO poolType = _poolTypes.Find(type => type.name == nodeData.nodeType.ToString());
            IPoolable poolGo = _poolManager.Pop(poolType);

            if (poolGo.GameObject != null && poolGo.GameObject.TryGetComponent(out Node node))
                node.Init(_judgement, nodeData);

            _nodeDatas.Dequeue();
        }
    }

    public void SetSpawnNode(List<NodeDataSO> nodeDatas)
    {
        ResetSpawner();
        foreach (NodeDataSO data in nodeDatas)
            _nodeDatas.Enqueue(data);
    }

    public void ResetSpawner()
    {
        _nodeDatas.Clear();
    }
}
