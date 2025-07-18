using AH.UI.Events;
using System;
using System.Collections;
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
    private PlayableDirector _sprayEmptyTimeline;
    private PlayableDirector _timelineSkipTimeline;

    private StageType _stageType;
    private bool _isOnPolice;

    private void Awake()
    {
        Attach();

        _startTimeline = transform.Find("ActivityStartTimeline").GetComponent<PlayableDirector>();
        _finishTimeline = transform.Find("FinishTimeline").GetComponent<PlayableDirector>();
        _activityEndTimelineNormal = transform.Find("ActivityEndTimeline_Normal").GetComponent<PlayableDirector>();
        _activityEndTimelinePolice = transform.Find("ActivityEndTimeline_Police").GetComponent<PlayableDirector>();
        _sprayEmptyTimeline = transform.Find("SprayEmptyTimeline").GetComponent<PlayableDirector>();
        _timelineSkipTimeline = transform.Find("TimelineSkipTimeline").GetComponent<PlayableDirector>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (mySubject.GameState == GameState.Timeline)
            {
                StartTimelineSkip();
            }
            else if (mySubject.GameState == GameState.Result)
            {
                ResultTimelineSkip();
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
        _stageType = dataController.stageData.stageType;

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
                StageEvent.SetProgressEvnet?.Invoke(false);

                _countdownTimeline?.Play();
                UIAnimationEvent.SetActiveCountDownAnimationEvnet?.Invoke(true);
            }
            else if (mySubject.GameState == GameState.Finish)
            {
                _finishTimeline.Play();
            }
            else if (mySubject.GameState == GameState.Result)
            {
                PlayableDirector playable = _isOnPolice ? _activityEndTimelinePolice : _activityEndTimelineNormal;
                playable = mySubject.IsPlayerWin ? playable : _sprayEmptyTimeline;
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
        mySubject.ChangeGameState(GameState.NextStage);
    }

    #region Timeline Skip

    private void StartTimelineSkip()
    {
        if (_startTimeline != null && _startTimeline.state == PlayState.Playing)
        {
            _startTimeline.Stop();
            _startTimeline.time = _startTimeline.duration;
            _startTimeline.Evaluate();

            GameManager.Instance.CharacterFade(0f, 0f);
            ActivityStartTimelineEnd();
        }
    }

    private void ResultTimelineSkip()
    {
        if (_activityEndTimelinePolice == null || _activityEndTimelineNormal == null || _sprayEmptyTimeline == null) return;
        if (_timelineSkipTimeline == null) return;

        PlayableDirector temp = _activityEndTimelineNormal.state == PlayState.Playing ? _activityEndTimelineNormal : _sprayEmptyTimeline;
        PlayableDirector playable = _activityEndTimelinePolice.state == PlayState.Playing ? _activityEndTimelinePolice : temp;

        playable.Stop();
        playable.time = playable.duration;
        playable.Evaluate();

        // 새 타임라인 실행
        _timelineSkipTimeline.Play();

        //StartCoroutine(ResultTimelineSkipRoutine(playable, () => _timelineSkipTimeline.Play()));
    }

    private IEnumerator ResultTimelineSkipRoutine(PlayableDirector director, Action callback = null)
    {
        director.time = director.duration;
        director.Evaluate();
        director.Play();
        yield return null;
        director.Stop();

        callback?.Invoke();
    }

    #endregion
}
