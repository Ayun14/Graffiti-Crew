using UnityEngine;

public class OneTouchGameRule : StageGameRule
{
    private int falseCount = 0;

    public override void NodeClear()
    {
        NodeSpawn();

        if (_graffitiRenderer != null && _nodeJudgement.currentNode != null)
            _graffitiRenderer.SetSprite(_nodeJudgement.currentNode.GetNodeDataSO().graffitiSprite);
    }

    public override void NodeFalse(Node node)
    {
        base.NodeFalse(node);

        if (++falseCount == 5)
        {
            _nodeSpawner.StopSpawn();
            AllNodeClear(); // 게임 강제 종료..
        }
    }
}
