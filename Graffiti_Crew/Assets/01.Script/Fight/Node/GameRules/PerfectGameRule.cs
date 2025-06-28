using UnityEngine;

public class PerfectGameRule : StageGameRule
{
    public override void NodeClear(Vector3 nodePos)
    {
        base.NodeClear(nodePos);

        _nodeSpawner.DequeueNode();

        if (_graffitiRenderer != null && _nodeJudgement.currentNode != null)
            _graffitiRenderer.SetSprite(_nodeJudgement.currentNode.GetNodeDataSO());
    }
}