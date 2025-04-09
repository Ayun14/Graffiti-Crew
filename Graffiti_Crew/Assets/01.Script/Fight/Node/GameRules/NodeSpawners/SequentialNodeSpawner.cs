public class SequentialNodeSpawner : NodeSpawner
{
    public override void NodeSpawn()
    {
        if (_nodeDatas == null) return;
        if (_nodeDatas.Count == 0)
        {
            _stageGameRule.AllNodeClear();
        }
        else
        {
            // PoolManager에서 가져오기
            NodeDataSO nodeData = _nodeDatas.Peek();
            PoolTypeSO poolType = _poolTypes.Find(type => type.name == nodeData.nodeType.ToString());
            IPoolable poolGo = _poolManager.Pop(poolType);
            _currentNode = poolGo.GameObject.GetComponent<Node>();

            if (poolGo.GameObject != null && poolGo.GameObject.TryGetComponent(out Node node))
                node.Init(_stageGameRule, _nodeJudgement, nodeData);

            DequeueNode();
        }
    }
}
