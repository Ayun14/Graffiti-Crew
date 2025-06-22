using AH.UI.Events;
using UnityEngine;
using UnityEngine.Playables;

public class ActivitySceneTimelineController : Observer<GameStateController>, INeedLoding
{
    [Header("Timeline")]
    [SerializeField] private PlayableDirector _countdownTimeline;
    private PlayableDirector _startTimeline;
    private PlayableDirector _finishTimeline;
    private PlayableDirector _activityEndTimelinePolice;
    private PlayableDirector _activityEndTimelineNormal;

    private bool _isOnPolice;

    private void Awake()
    {
        Attach();

        _startTimeline = transform.Find("ActivityStartTimeline").GetComponent<PlayableDirector>();
        _finishTimeline = transform.Find("FinishTimeline").GetComponent<PlayableDirector>();
        _activityEndTimelineNormal = transform.Find("ActivityEndTimeline_Normal").GetComponent<PlayableDirector>();
        _activityEndTimelinePolice = transform.Find("ActivityEndTimeline_Police").GetComponent<PlayableDirector>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (mySubject.GameState == GameState.Timeline && _startTimeline.state == PlayState.Playing)
            {
                _startTimeline.Stop();
                mySubject.ChangeGameState(GameState.Countdown);
            }
        }
    }

    private void OnDestroy()
    {
        Detach();
    }

    public void LodingHandle(DataController dataController)
    {
        _isOnPolice = dataController.stageData.isOnPolice;

        dataController.SuccessGiveData();
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

                PlayableDirector playable = _isOnPolice ? _activityEndTimelinePolice : _activityEndTimelineNormal;
                playable.Play();
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
