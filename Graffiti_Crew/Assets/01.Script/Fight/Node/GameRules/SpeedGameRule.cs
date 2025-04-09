using UnityEngine;

public class SpeedGameRule : StageGameRule
{
    public override void NodeClear()
    {
        NodeSpawn();

        if (_graffitiRenderer != null && _nodeJudgement.currentNode != null)
            _graffitiRenderer.SetSprite(_nodeJudgement.currentNode.GetNodeDataSO().graffitiSprite);
    }
}
