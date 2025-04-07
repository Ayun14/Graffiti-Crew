using UnityEngine;
using AH.UI.Events;
using AH.UI.ViewModels;
using AH.UI.Models;
using System.Threading.Tasks;

public class RequestSceneDataController : DataController
{
    protected override void NotifyHandleChild()
    {

    }

    protected override void FindDatas()
    {
        stageData = Resources.Load("StageData/" + stageSO.GetLoadRequestName()) as StageDataSO;
    }

    protected async override void FinishGiveData()
    {
        // Fade
        PresentationEvents.SetFadeEvent?.Invoke(true);
        await Task.Delay(1100);
        PresentationEvents.FadeInOutEvent?.Invoke(true);

        mySubject.ChangeGameState(GameState.Talk);
        //mySubject.ChangeGameState(GameState.Graffiti);
    }
}
