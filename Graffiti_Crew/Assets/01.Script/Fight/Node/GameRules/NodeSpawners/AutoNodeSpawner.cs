using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoNodeSpawner : NodeSpawner
{
    private List<Node> _spawnedNode = new();

    public override void NodeSpawn()
    {
        if (_nodeDatas.Count == 0)
            _stageGameRule.AllNodeClear();

        StopCoroutine("NodeSpawnRoutine");
        StartCoroutine("NodeSpawnRoutine");
    }

    private IEnumerator NodeSpawnRoutine()
    {
        if (_spawnedNode != null && _spawnedNode.Count > 0)
        {
            foreach (Node node in _spawnedNode)
                if (node != null) node.PushObj();
        }

        List<NodeDataSO> nodeDatasCopy = new List<NodeDataSO>(_nodeDatas);

        for (int i = 0; i < nodeDatasCopy.Count; ++i)
        {
            NodeDataSO nodeData = nodeDatasCopy[i];

            // PoolManager에서 가져오기
            PoolTypeSO poolType = _poolTypes.Find(type => type.name == nodeData.nodeType.ToString());
            IPoolable poolGo = _poolManager.Pop(poolType);

            if (poolGo.GameObject != null && poolGo.GameObject.TryGetComponent(out Node node))
            {
                _spawnedNode.Add(node);
                _currentNode = node;
                node.Init(_stageGameRule, _nodeJudgement, nodeData);
                node.StartVisibleRoutine();
                yield return new WaitForSeconds(node.fadeTime);
            }
        }
    }
}
