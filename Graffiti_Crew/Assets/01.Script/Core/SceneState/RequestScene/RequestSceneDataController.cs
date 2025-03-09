using UnityEngine;

public class RequestSceneDataController : DataController
{
    protected override void NotifyHandleChild()
    {

    }

    protected override void FindDatas()
    {
        _stageData = Resources.Load("StageData/" + stageSO.GetLoadRequestName()) as StageDataSO;
    }


    protected override void GiveData()
    {
        base.GiveData();

        //mySubject.ChangeGameState(GameState.Talk);
        mySubject.ChangeGameState(GameState.Graffiti);
    }
}
