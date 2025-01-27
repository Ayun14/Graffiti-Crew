using UnityEngine.Playables;

public class FightSceneTimelineController : Observer<GameStateController>, INeedLoding
{
    private PlayableDirector _beforeFightTimeline;
    private PlayableDirector _finishtTimeline;

    private void Awake()
    {
        Attach();

        _beforeFightTimeline = transform.Find("BeforeFightTimeline").GetComponent<PlayableDirector>();
        _finishtTimeline = transform.Find("FinishTimeline").GetComponent<PlayableDirector>();
    }

    private void OnDestroy()
    {
        Detach();
    }

    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            if (mySubject.GameState == GameState.Timeline)
                _beforeFightTimeline.Play();

            if (mySubject.GameState == GameState.Finish)
                _finishtTimeline.Play();
        }
    }

    public void BeforeFightTimelineEnd()
    {
        if (mySubject != null)
        {
            mySubject.ChangeGameState(GameState.CountDown);
        }
    }

    public void FinishTimelineEnd()
    {
        if (mySubject != null)
        {
            mySubject.ChangeGameState(GameState.Result);
        }
    }

    public void LodingHandle(StageDataSO stageData)
    {
        // 대사 장착 및 등등....
    }
}
