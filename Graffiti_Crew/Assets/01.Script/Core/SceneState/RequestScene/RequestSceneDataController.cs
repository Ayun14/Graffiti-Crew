using UnityEngine;

public class RequestSceneDataController : DataController
{
    protected override void NotifyHandleChild()
    {

    }

    protected override void FindDatas()
    {
        stageData = Resources.Load("StageData/" + stageSO.GetLoadRequestName()) as StageDataSO;
    }

    protected override void FinishGiveData()
    {
        //mySubject.ChangeGameState(GameState.Talk);
        mySubject.ChangeGameState(GameState.Graffiti);
    }
}
