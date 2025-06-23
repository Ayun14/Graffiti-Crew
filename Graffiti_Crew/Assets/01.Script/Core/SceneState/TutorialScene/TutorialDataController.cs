using AH.UI.Events;
using System.Threading.Tasks;
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

    protected async override void FinishGiveData()
    {
        StageEvent.SetActiveFightViewEvent?.Invoke(false);

        PresentationEvents.SetFadeEvent?.Invoke(true);
        await Task.Delay(1100);
        PresentationEvents.FadeInOutEvent?.Invoke(true);

        mySubject.ChangeGameState(GameState.Dialogue);
    }
}
