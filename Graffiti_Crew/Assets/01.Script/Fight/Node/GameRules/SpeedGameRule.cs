using UnityEngine;

public class SpeedGameRule : StageGameRule
{
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

        if (!mySubject.IsBlind && Random.Range(0, 100f) < 30)
        {
            mySubject?.InvokeBlindEvent();

            // Sound
            GameManager.Instance.SoundSystemCompo.PlaySFX(SoundType.Throw_Egg);
        }
    }
}
