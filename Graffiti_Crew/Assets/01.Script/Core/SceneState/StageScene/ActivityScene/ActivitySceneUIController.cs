using AH.UI.Events;
using UnityEngine;

public class ActivitySceneUIController : Observer<GameStateController>
{
    public override void NotifyHandle()
    {

    }

    public void SetResultUI()
    {
        StageEvent.ShowResultViewEvent?.Invoke(true, mySubject.IsPlayerWin);
    }
}
