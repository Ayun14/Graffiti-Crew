using UnityEngine;

public class SpeedGameRule : StageGameRule
{
    public override void NodeClear()
    {
        base.NodeClear();

        NodeSpawn();

        if (_graffitiRenderer != null && _nodeJudgement.currentNode != null)
            _graffitiRenderer.SetSprite(_nodeJudgement.currentNode.GetNodeDataSO().graffitiSprite);
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
