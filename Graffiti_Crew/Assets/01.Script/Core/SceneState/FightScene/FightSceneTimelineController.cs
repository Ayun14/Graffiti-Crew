using AH.UI.Events;
using Unity.Cinemachine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine;

public class FightSceneTimelineController : Observer<GameStateController>, INeedLoding
{
    [SerializeField] private CinemachineCamera _playerGraffitiCam;
    [SerializeField] private CinemachineCamera _rivalGraffitiCam;

    private PlayableDirector _beforeFightTimeline;
    private PlayableDirector _finishTimeline;
    private PlayableDirector _resultTimeline;
    private DialogueUIController _dialogueUIController;

    private void Awake()
    {
        Attach();

        _beforeFightTimeline = transform.Find("BeforeFightTimeline").GetComponent<PlayableDirector>();
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
                _resultTimeline.Play();
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

    public void SetWinnerCam()
    {
        TimelineAsset timeline = _resultTimeline.playableAsset as TimelineAsset;

        foreach (var track in timeline.GetOutputTracks())
        {
            if (track is CinemachineTrack && track.name == "GraffitiCam")
            {
                CinemachineCamera newCamera = mySubject.IsPlayerWin ? _playerGraffitiCam : _rivalGraffitiCam;
                _resultTimeline.SetGenericBinding(track, newCamera);
                return;
            }
        }
    }

    public void LodingHandle(StageDataSO stageData)
    {
        _dialogueUIController.dialogueDataReader_KR = stageData.dialogueData_KR;
        _dialogueUIController.dialogueDataReader_EN = stageData.dialogueData_EN;
    }
}
