using UnityEngine;

public class FightScenePlayerController : Observer<GameStateController>, INeedLoding
{
    // ���⿡ �÷��̾� �� �ִϸ��̼�... ��ġ.. ��� �����ϱ�

    public void LodingHandle(StageDataSO stageData)
    {

    }

    private void Awake()
    {
        Attach();
    }

    private void OnDestroy()
    {
        Detach();
    }

    public override void NotifyHandle()
    {

    }
}
