using UnityEngine;
using UnityEngine.Playables;

public class FightSceneTimelineController : Observer<GameStateController>
{
    [SerializeField] private PlayableDirector _beforeFightTimeline;
    [SerializeField] private PlayableDirector _countdownTimeline;
    [SerializeField] private PlayableDirector _endFightTimeline;
    private PlayableDirector _finishTimeline;
    private PlayableDirector _resultTimeline;

    private void Awake()
    {
        Attach();

        _finishTimeline = transform.Find("FinishTimeline").GetComponent<PlayableDirector>();
        _resultTimeline = transform.Find("ResultTimeline").GetComponent<PlayableDirector>();
    }

    private void OnDestroy()
    {
        Detach();
    }

    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            if (mySubject.GameState == GameState.Timeline) {
                UIAnimationEvent.SetActiveStartAnimationEvnet?.Invoke(true);
                _beforeFightTimeline.Play();
            }

            if (mySubject.GameState == GameState.Finish) {
                _finishTimeline.Play();
            }

            if (mySubject.GameState == GameState.Result)
                _resultTimeline.Play();
        }
    }

    public void BeforeFightTimelineEnd()
    {
        if (mySubject != null)
        {
            mySubject.ChangeGameState(GameState.Countdown);
            UIAnimationEvent.SetActiveCountDownAnimationEvnet?.Invoke(true);
            UIAnimationEvent.SetActiveStartAnimationEvnet?.Invoke(false);
            _countdownTimeline?.Play();
        }
    }

    public void CountdownTimelineEnd()
    {
        if (mySubject != null)
        {
            mySubject.ChangeGameState(GameState.Fight);
            UIAnimationEvent.SetActiveCountDownAnimationEvnet?.Invoke(false);
        }
    }

    public void FinishTimelineEnd()
    {
        if (mySubject != null)
        {
            mySubject.ChangeGameState(GameState.Result);
        }
    }

    public void ResultTimelineEnd()
    {
        UIAnimationEvent.SetActiveEndAnimationEvnet?.Invoke(true);
        _endFightTimeline?.Play();
    }
}
