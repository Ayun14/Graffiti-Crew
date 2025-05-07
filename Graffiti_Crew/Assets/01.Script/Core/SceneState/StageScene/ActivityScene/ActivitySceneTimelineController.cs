using UnityEngine;
using UnityEngine.Playables;

public class ActivitySceneTimelineController : Observer<GameStateController>
{
    [SerializeField] private PlayableDirector _countdownTimeline;
    private PlayableDirector _startTimeline;
    private PlayableDirector _finishTimeline;
    private PlayableDirector _activityEndTimeline;

    private void Awake()
    {
        Attach();

        _startTimeline = transform.Find("ActivityStartTimeline").GetComponent<PlayableDirector>();
        _finishTimeline = transform.Find("FinishTimeline").GetComponent<PlayableDirector>();
        _activityEndTimeline = transform.Find("ActivityEndTimeline").GetComponent<PlayableDirector>();

        UIAnimationEvent.SetActiveStartAnimationEvnet?.Invoke(false);
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
            else if (mySubject.GameState == GameState.Countdown)
            {
                _countdownTimeline?.Play();
            }
            else if (mySubject.GameState == GameState.Finish)
            {
                _finishTimeline.Play();
            }
            else if (mySubject.GameState == GameState.Result)
            {
                _activityEndTimeline.Play();
            }
        }
    }

    public void ActivityStartTimelineEnd()
    {
        mySubject.ChangeGameState(GameState.Countdown);
    }

    public void CounddownTimelineEnd()
    {
        mySubject.ChangeGameState(GameState.Fight);
    }

    public void FinishTimelineEnd()
    {
        mySubject.ChangeGameState(GameState.Result);
    }
}
