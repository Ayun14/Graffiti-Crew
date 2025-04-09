using System.Collections;
using UnityEngine;

public class AutoNodeSpawner : NodeSpawner
{
    public override void NodeSpawn()
    {
        if (_nodeDatas.Count == 0)
            _stageGameRule.AllNodeClear();

        StopAllCoroutines();
        StartCoroutine(NodeSpawnRoutine());
    }

    private IEnumerator NodeSpawnRoutine()
    {
        foreach (NodeDataSO nodeData in _nodeDatas)
        {
            // PoolManager에서 가져오기
            PoolTypeSO poolType = _poolTypes.Find(type => type.name == nodeData.nodeType.ToString());
            IPoolable poolGo = _poolManager.Pop(poolType);
            _currentNode = poolGo.GameObject.GetComponent<Node>();

            if (poolGo.GameObject != null && poolGo.GameObject.TryGetComponent(out Node node))
            {
                node.Init(_stageGameRule, _nodeJudgement, nodeData);
                node.StartVisibleRoutine();
                yield return new WaitForSeconds(node.fadeTime);
            }
        }
    }
}
