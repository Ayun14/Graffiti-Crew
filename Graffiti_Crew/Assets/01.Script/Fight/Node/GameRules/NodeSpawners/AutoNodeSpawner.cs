using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoNodeSpawner : NodeSpawner
{
    private List<Node> _spawnedNode = new();
    private Coroutine _spawnRoutine;

    public override void NodeSpawn()
    {
        if (_nodeDatas.Count == 0)
            _stageGameRule.AllNodeClear();

        if (_spawnRoutine != null) StopCoroutine(_spawnRoutine);
        _spawnRoutine = StartCoroutine(NodeSpawnRoutine());
    }

    private IEnumerator NodeSpawnRoutine()
    {
        // 기존 노드 제거
        foreach (Node node in _spawnedNode)
        {
            node?.PushObj();
        }
        _spawnedNode.Clear();

        List<NodeDataSO> nodeDatasCopy = new List<NodeDataSO>(_nodeDatas);
        foreach (NodeDataSO nodeData in nodeDatasCopy)
        {
            PoolTypeSO poolType = _poolTypes.Find(type => type.name == nodeData.nodeType.ToString());
            IPoolable poolGo = _poolManager.Pop(poolType);

            if (poolGo.GameObject.TryGetComponent(out Node node))
            {
                _spawnedNode.Add(node);
                _currentNode = node;
                node.Init(_stageGameRule, _nodeJudgement, nodeData);
                node.StartVisibleRoutine(() =>
                {
                    _spawnedNode.Remove(node);
                });

                float waitTime = node.fadeTime + (node.visibleTime / 2);
                yield return new WaitForSeconds(waitTime);
            }
        }

        yield return new WaitUntil(() => _spawnedNode.Count != 0);
        NodeSpawn();
    }

    public override void StopSpawn()
    {
        if (_spawnRoutine != null) StopCoroutine(_spawnRoutine);

        foreach (Node node in _spawnedNode)
            node?.PushObj();
    }
}
