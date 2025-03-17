using UnityEngine;

public class TutorialDataController : DataController
{
    protected override void NotifyHandleChild()
    {
    }

    protected override void FindDatas()
    {
        stageData = Resources.Load("StageData/" + stageSO.GetLoadStageName()) as StageDataSO;
        Debug.Log(stageSO.GetLoadStageName());
    }

    protected override void FinishGiveData()
    {
        mySubject.ChangeGameState(GameState.Dialogue);
    }
}
