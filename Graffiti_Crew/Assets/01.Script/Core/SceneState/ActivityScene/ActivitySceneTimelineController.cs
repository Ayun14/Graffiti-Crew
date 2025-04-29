using UnityEngine.Playables;

public class ActivitySceneTimelineController : Observer<GameStateController>
{
    private PlayableDirector _startTimeline;
    private PlayableDirector _activityEndTimeline;

    private void Awake()
    {
        Attach();

        _startTimeline = transform.Find("ActivityStartTimeline").GetComponent<PlayableDirector>();
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
        }
    }

    public void ActivityStartTimelineEnd()
    {
        mySubject.ChangeGameState(GameState.Graffiti);
    }
}
