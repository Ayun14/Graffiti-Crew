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
                UIAnimationEvent.SetActiveEndAnimationEvnet?.Invoke(false);
                UIAnimationEvent.SetActiveStartAnimationEvnet?.Invoke(false);
                UIAnimationEvent.SetActiveCountDownAnimationEvnet?.Invoke(false);
                _startTimeline.Play();
            }
            else if (mySubject.GameState == GameState.Countdown)
            {
                _countdownTimeline?.Play();
                UIAnimationEvent.SetActiveCountDownAnimationEvnet?.Invoke(true);
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
        UIAnimationEvent.SetActiveCountDownAnimationEvnet?.Invoke(false);
    }

    public void FinishTimelineEnd()
    {
        mySubject.ChangeGameState(GameState.Result);
    }
}
