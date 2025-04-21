using UnityEngine;
using UnityEngine.Playables;

public class FightSceneTimelineController : Observer<GameStateController>
{
    [SerializeField] private PlayableDirector _beforeFightTimeline;
    private PlayableDirector _finishTimeline;
    private PlayableDirector _resultTimeline;
    private PlayableDirector _endFightTimeline;
    private DialogueUIController _dialogueUIController;

    private void Awake()
    {
        Attach();

        _finishTimeline = transform.Find("FinishTimeline").GetComponent<PlayableDirector>();
        _resultTimeline = transform.Find("ResultTimeline").GetComponent<PlayableDirector>();
        _endFightTimeline = transform.Find("EndFightTimeline").GetComponent<PlayableDirector>();
        _dialogueUIController = transform.Find("FightUI").GetComponent<DialogueUIController>();
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
                _finishTimeline.Play();

            if (mySubject.GameState == GameState.Result)
                _resultTimeline.Play();
        }
    }

    public void BeforeFightTimelineEnd()
    {
        if (mySubject != null)
        {
            mySubject.ChangeGameState(GameState.Fight);
            UIAnimationEvent.SetActiveStartAnimationEvnet?.Invoke(false);
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
        _endFightTimeline?.Play();
    }

    public void DialogueRival()
    {
        _dialogueUIController.StartDialogue(1, 1);
    }

    public void DialoguePlayer()
    {
        _dialogueUIController.StartDialogue(2, 2);
    }
}
