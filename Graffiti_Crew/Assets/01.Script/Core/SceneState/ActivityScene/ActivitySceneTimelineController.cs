using UnityEngine.Playables;

public class ActivitySceneTimelineController : Observer<GameStateController>
{
    private PlayableDirector _startTimeline;
    private PlayableDirector _finishTimeline;
    private PlayableDirector _activityEndTimeline;

    private void Awake()
    {
        Attach();

        _startTimeline = transform.Find("ActivityStartTimeline").GetComponent<PlayableDirector>();
        _finishTimeline = transform.Find("FinishTimeline").GetComponent<PlayableDirector>();
        _activityEndTimeline = transform.Find("ActivityEndTimeline").GetComponent<PlayableDirector>();
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
            {
                _startTimeline.Play();
            }

            if (mySubject.GameState == GameState.Finish)
            {
                _finishTimeline.Play();
            }

            if (mySubject.GameState == GameState.Result)
            {
                _activityEndTimeline.Play();
            }
        }
    }

    public void ActivityStartTimelineEnd()
    {
        mySubject.ChangeGameState(GameState.Graffiti);
    }

    public void FinishTimelineEnd()
    {
        mySubject.ChangeGameState(GameState.Result);
    }
}
