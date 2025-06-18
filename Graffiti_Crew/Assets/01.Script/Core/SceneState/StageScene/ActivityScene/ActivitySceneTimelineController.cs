using AH.UI.Events;
using UnityEngine;
using UnityEngine.Playables;

public class ActivitySceneTimelineController : Observer<GameStateController>
{
    [Header("Timeline")]
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
                // Film UI
                UIAnimationEvent.SetFilmDirectingEvent(true);

                // Other UI
                UIAnimationEvent.SetActiveEndAnimationEvnet?.Invoke(false);
                UIAnimationEvent.SetActiveStartAnimationEvnet?.Invoke(false);
                UIAnimationEvent.SetActiveCountDownAnimationEvnet?.Invoke(false);

                _startTimeline.Play();
            }
            else if (mySubject.GameState == GameState.Countdown)
            {
                StageEvent.SetViewEvnet?.Invoke(false);

                _countdownTimeline?.Play();
                UIAnimationEvent.SetActiveCountDownAnimationEvnet?.Invoke(true);
            }
            else if (mySubject.GameState == GameState.Finish)
            {
                _finishTimeline.Play();
            }
            else if (mySubject.GameState == GameState.Result)
            {
                UIAnimationEvent.SetFilmDirectingEvent(true);
                _activityEndTimeline.Play();
            }
        }
    }

    public void ActivityStartTimelineEnd()
    {
        UIAnimationEvent.SetFilmDirectingEvent(false);
        mySubject.ChangeGameState(GameState.Countdown);
    }

    public void CountdownTimelineEnd()
    {
        mySubject.ChangeGameState(GameState.Fight);
        UIAnimationEvent.SetActiveCountDownAnimationEvnet?.Invoke(false);
    }

    public void FinishTimelineEnd()
    {
        mySubject.ChangeGameState(GameState.Result);
    }

    public void ResultTimelineEnd()
    {
        UIAnimationEvent.SetFilmDirectingEvent(false);
    }
}
