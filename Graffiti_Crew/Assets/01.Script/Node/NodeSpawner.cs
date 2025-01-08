using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeSpawner : MonoBehaviour
{
    private Queue<GameObject> _nodes = new Queue<GameObject>();

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
        if (_nodes == null) return;
        if (_nodes.Count == 0)
        {
            Debug.Log("모든 노드 클리어");
        }
        else
        {
            // PoolManager에서 가져오기
            GameObject go = Instantiate(_nodes.Dequeue()); // Pop
            if (go.TryGetComponent(out Node node))
                node.Init(_judgement);
        }
    }

    public void SetSpawnNode(List<GameObject> nodes)
    {
        ResetSpawner();
        foreach (GameObject go in nodes)
            _nodes.Enqueue(go);
    }

    public void ResetSpawner()
    {
        _nodes.Clear();
    }
}
