using UnityEngine;

public class FightScenePlayerController : Observer<GameStateController>, INeedLoding
{
    // 여기에 플레이어 뭐 애니메이션... 위치.. 등등 구현하기

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
