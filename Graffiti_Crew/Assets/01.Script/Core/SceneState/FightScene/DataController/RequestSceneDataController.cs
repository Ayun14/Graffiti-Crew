using UnityEngine;

public class RequestSceneDataController : DataController
{
    protected override void NotifyHandleChild()
    {

    }


    protected override void GiveData()
    {
        base.GiveData();

        //mySubject.ChangeGameState(GameState.Talk);
        mySubject.ChangeGameState(GameState.Graffiti);
    }
}
