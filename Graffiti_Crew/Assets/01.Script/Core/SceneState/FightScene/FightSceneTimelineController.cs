using AH.UI.Events;
using UnityEngine.Playables;

public class FightSceneTimelineController : Observer<GameStateController>, INeedLoding
{
    private PlayableDirector _beforeFightTimeline;
    private PlayableDirector _finishtTimeline;
    private DialogueUIController _dialogueUIController;

    private void Awake()
    {
        Attach();

        _beforeFightTimeline = transform.Find("BeforeFightTimeline").GetComponent<PlayableDirector>();
        _finishtTimeline = transform.Find("FinishTimeline").GetComponent<PlayableDirector>();
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
                _finishtTimeline.Play();
        }
    }

    public void BeforeFightTimelineEnd()
    {
        if (mySubject != null)
        {
            mySubject.ChangeGameState(GameState.CountDown);
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
        _dialogueUIController.StartDialogue(1, 1, PlayTimeline);
    }

    public void DialoguePlayer()
    {
        _dialogueUIController.StartDialogue(2, 2, PlayTimeline);
    }

    public void PlayTimeline()
    {
        _beforeFightTimeline.Play();
    }

    public void LodingHandle(StageDataSO stageData)
    {
        _dialogueUIController.dialogueDataReader_KR = stageData.dialogueData_KR;
        _dialogueUIController.dialogueDataReader_EN = stageData.dialogueData_EN;
    }
}
