using UnityEngine;

public class FightSceneNPCController : Observer<GameStateController>
{
    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            if (mySubject.GameState == GameState.Timeline)
            {
                AnimationEvent.SetAnimation?.Invoke(10, AnimationEnum.People_Idle);
                AnimationEvent.SetAnimation?.Invoke(11, AnimationEnum.People_Idle);
                AnimationEvent.SetAnimation?.Invoke(12, AnimationEnum.People_Idle);
            }
            else if (mySubject.GameState == GameState.Finish)
            {
                AnimationEvent.SetAnimation?.Invoke(10, AnimationEnum.People_Clap);
                AnimationEvent.SetAnimation?.Invoke(11, AnimationEnum.People_Clap);
                AnimationEvent.SetAnimation?.Invoke(12, AnimationEnum.People_Clap);
            }
        }
    }
}
