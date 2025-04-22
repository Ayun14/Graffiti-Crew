public class SequentialNodeSpawner : NodeSpawner
{
    public override void NodeSpawn()
    {
        if (_nodeDatas == null) return;

        NodeDataSO nodeData = _nodeDatas.Peek();
        PoolTypeSO poolType = _poolTypes.Find(type => type.name == nodeData.nodeType.ToString());
        IPoolable poolGo = _poolManager.Pop(poolType);
        _currentNode = poolGo.GameObject.GetComponent<Node>();

        if (poolGo.GameObject != null && poolGo.GameObject.TryGetComponent(out Node node))
            node.Init(_stageGameRule, _nodeJudgement, nodeData);

        DequeueNode();
    }

    public override void StopSpawn()
    {
        if (_currentNode != null)
            _currentNode.PushObj();
    }
}
