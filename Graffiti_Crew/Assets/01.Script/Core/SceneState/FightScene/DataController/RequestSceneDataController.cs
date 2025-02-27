using UnityEngine;

public class RequestSceneDataController : DataController
{
    protected override void NotifyHandleChild()
    {

    }


    protected override void GiveData()
    {
        base.GiveData();

        Debug.Log("GiveData");
        mySubject.ChangeGameState(GameState.Talk);
        //mySubject.ChangeGameState(GameState.Graffiti);
    }
}
