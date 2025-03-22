using UnityEngine;
using AH.UI.Events;
using AH.UI.ViewModels;
using AH.UI.Models;

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
        // Fade
        PresentationEvents.SetFadeEvent?.Invoke(true);
        PresentationEvents.FadeInOutEvent?.Invoke(true);

        //mySubject.ChangeGameState(GameState.Talk);
        mySubject.ChangeGameState(GameState.Graffiti);
    }
}
