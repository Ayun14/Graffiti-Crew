using UnityEngine;

public class TutorialDataController : DataController
{
    protected override void NotifyHandleChild()
    {
    }

    protected override void FindDatas()
    {
        _stageData = Resources.Load("StageData/" + stageSO.GetLoadStageName()) as StageDataSO;
    
    }

    protected override void GiveData()
    {
        base.GiveData();

        mySubject.ChangeGameState(GameState.Dialogue);
    }
}
