using UnityEngine;

public class OneTouchGameRule : StageGameRule
{
    private int falseCount = 0;

    public override void NodeClear(Vector3 nodePos)
    {
        base.NodeClear(nodePos);

        NodeSpawn();

        if (_graffitiRenderer != null && _nodeJudgement.currentNode != null)
            _graffitiRenderer.SetSprite(_nodeJudgement.currentNode.GetNodeDataSO());
    }

    public override void NodeFalse()
    {
        base.NodeFalse();

        if (++falseCount == 5)
            AllNodeClear(); // ���� ���� ����..
    }
}
