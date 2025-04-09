using UnityEngine;

public class PerfectGameRule : StageGameRule
{
    public override void NodeClear()
    {
        _nodeSpawner.DequeueNode();

        if (_graffitiRenderer != null && _nodeJudgement.currentNode != null)
            _graffitiRenderer.SetSprite(_nodeJudgement.currentNode.GetNodeDataSO().graffitiSprite);
    }
}