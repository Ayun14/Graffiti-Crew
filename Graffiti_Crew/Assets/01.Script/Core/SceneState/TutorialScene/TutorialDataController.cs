using UnityEngine;

public class TutorialDataController : DataController
{
    protected override void NotifyHandleChild()
    {
    }

    protected override void FindDatas()
    {
        stageData = Resources.Load("StageData/" + stageSO.GetLoadStageName()) as StageDataSO;
    }

    protected override void FinishGiveData()
    {
        mySubject.ChangeGameState(GameState.Dialogue);
    }
}
