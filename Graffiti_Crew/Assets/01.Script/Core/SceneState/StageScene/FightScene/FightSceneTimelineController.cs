using AH.UI.Events;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (mySubject.GameState == GameState.Timeline)
            {
                StartTimelineSkip();
            }
            //else if (mySubject.GameState == GameState.Result)
            //{
            //    ResultTimelineSkip();
            //}
        }
    }

    private void StartTimelineSkip()
    {
        if (_beforeFightTimeline != null && _beforeFightTimeline.state == PlayState.Playing)
        {
            _beforeFightTimeline.time = _beforeFightTimeline.duration;
            _beforeFightTimeline.Evaluate();
            _beforeFightTimeline.Stop();

            BeforeFightTimelineEnd();
        }
    }

    private void ResultTimelineSkip()
    {
        if (_resultTimeline == null || _endFightTimeline == null) return;

        if (_resultTimeline.state == PlayState.Playing)
        {
            _resultTimeline.Stop();
            _resultTimeline.time = _resultTimeline.duration;
            _resultTimeline.Evaluate();

            UIAnimationEvent.SetActiveEndAnimationEvnet?.Invoke(true);
            _endFightTimeline.Play();
        }

        if (_endFightTimeline.state == PlayState.Playing)
        {
            //_endFightTimeline.Stop();
            _endFightTimeline.time = _endFightTimeline.duration;
            _endFightTimeline.Evaluate();
        }

        EndFightTimelineEnd();
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

            if (mySubject.GameState == GameState.Countdown) {
                StageEvent.SetProgressEvnet?.Invoke(true);
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
        GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Fight_Result);
        GameManager.Instance.SoundSystemCompo.PlayBGM(SoundType.Drum_Roll);

        UIAnimationEvent.SetActiveEndAnimationEvnet?.Invoke(true);
        _endFightTimeline?.Play();
    }

    public void EndFightTimelineEnd()
    {
        mySubject.ChangeGameState(GameState.NextStage);
    }

    public void LoopResultTimeLine() {
        _endFightTimeline.time = 6.8f;
        _endFightTimeline.Evaluate(); // 바로 상태 반영
        _endFightTimeline.Play();     // 다시 재생
    }
}
