using System.Threading.Tasks;
using UnityEngine;

public class ComputerDataController : DataController {
    protected override void NotifyHandleChild() {
    }

    protected override void FindDatas() {
        stageData = Resources.Load("StageData/" + stageSO.GetLoadStageName()) as StageDataSO;
    }

    protected async override void FinishGiveData() {
        PresentationEvents.SetFadeEvent?.Invoke(true);
        await Task.Delay(1100);
        PresentationEvents.FadeInOutEvent?.Invoke(true);
        Debug.Log("½ÇÇà");
        mySubject.ChangeGameState(GameState.Computer);
    }
}
