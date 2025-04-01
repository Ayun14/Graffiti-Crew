using AH.UI.Events;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class FightSceneTimelineController : Observer<GameStateController>, INeedLoding
{
    [SerializeField] private PlayableDirector _beforeFightTimeline;
    private PlayableDirector _finishTimeline;
    private PlayableDirector _resultTimeline;
    private DialogueUIController _dialogueUIController;

    private void Awake()
    {
        Attach();

        _finishTimeline = transform.Find("FinishTimeline").GetComponent<PlayableDirector>();
        _resultTimeline = transform.Find("ResultTimeline").GetComponent<PlayableDirector>();
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
            {
                _resultTimeline.Play();
            }
        }
    }

    public void BeforeFightTimelineEnd()
    {
        if (mySubject != null)
        {
            mySubject.ChangeGameState(GameState.Fight);
            StageEvent.SetActiveStartAnimation?.Invoke(false);
        }
    }

    public void FinishTimelineEnd()
    {
        if (mySubject != null)
        {
            mySubject.ChangeGameState(GameState.Result);
        }
    }

    public void DialogueRival()
    {
        _dialogueUIController.StartDialogue(1, 1);
    }

    public void DialoguePlayer()
    {
        _dialogueUIController.StartDialogue(2, 2);
    }

    public void LodingHandle(DataController dataController)
    {
        dataController.SuccessGiveData();
    }
}
