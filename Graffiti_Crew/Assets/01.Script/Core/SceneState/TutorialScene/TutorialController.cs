using AH.UI.Events;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : Observer<GameStateController>, INeedLoding
{
    [SerializeField] private GameObject _level;
    [SerializeField] private GameObject _hangout;
    [SerializeField] private GameObject _mainCam;

    [SerializeField] private DialogueController _dialogueController;
    [SerializeField] private DialogueUIController _dialogueUIController;
    [SerializeField] private StoryDialogueSO _tutorialDialogueSO;

    private int _dialogueNum = 0;
    private int _clearNode = 0;

    private int _tutorialStartIndex = 81;


    private void Awake()
    {
        Attach();
    }

    private void OnDestroy()
    {
        Detach();
    }

    public override void NotifyHandle()
    {
        if (mySubject != null)
        {
            if (mySubject.GameState == GameState.Dialogue)
            {
                if(_dialogueNum == 0)
                    GameManager.Instance.SoundSystemCompo.PlayBGM(SoundType.Rain);

                _dialogueUIController.ChangeDialogueUI?.Invoke(true);
                StageEvent.SetActiveFightViewEvent?.Invoke(false);

                NPCSO dialogue = _tutorialDialogueSO.storyList[_dialogueNum];
                _dialogueUIController.ChangeDialogueUI?.Invoke(true);
                _dialogueController.StartDialogue(dialogue.startIndex, dialogue.endIndex, DialogueEnd);

                // Cursor
                GameManager.Instance.SetCursor(false);
            }

            if(mySubject.GameState == GameState.Tutorial)
            {
                _dialogueUIController.ChangeDialogueUI?.Invoke(false);
                DialogueEvent.ShowDialougeViewEvent?.Invoke(false);

                //_dialogueUIController.OnEndTyping?.Invoke(false);

                _dialogueController.StartDialogue(_tutorialStartIndex, _tutorialStartIndex + 9, null);

                //SetMiniDialogue();

                // Cursor
                GameManager.Instance.SetCursor(true);
            }
        }
    }

    private async void DialogueEnd()
    {

        if(_dialogueNum == _tutorialDialogueSO.storyList.Count)
            _hangout.SetActive(false);

        PresentationEvents.FadeInOutEvent?.Invoke(false);
        await Task.Delay(1100);
        DialogueEvent.ShowDialougeViewEvent?.Invoke(false);

        _dialogueNum++;

        if (_dialogueNum == 1)
        {
            NPCSO dialogue = _tutorialDialogueSO.storyList[_dialogueNum];

            await Task.Delay(1100);
            PresentationEvents.FadeInOutEvent?.Invoke(true);
            GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Rain);
            GameManager.Instance.SoundSystemCompo.PlayBGM(SoundType.Tutorial);

            _dialogueController.StartDialogue(dialogue.startIndex, dialogue.endIndex, DialogueEnd);
        }
        else if (_dialogueNum == 2)
        {
            PresentationEvents.FadeInOutEvent?.Invoke(false);
            await Task.Delay(2100);
            _level.SetActive(true);
            PresentationEvents.FadeInOutEvent?.Invoke(true);

            mySubject.ChangeGameState(GameState.Tutorial);
        }
        else if (_dialogueNum == 3)
        {
            _mainCam.SetActive(false);
            NPCSO dialogue = _tutorialDialogueSO.storyList[_dialogueNum];

            _level.SetActive(false);
            _hangout.SetActive(true);
            await Task.Delay(1100);
            PresentationEvents.FadeInOutEvent?.Invoke(true);

            _dialogueController.StartDialogue(dialogue.startIndex, dialogue.endIndex, DialogueEnd);
        }
        else if(_dialogueNum == _tutorialDialogueSO.storyList.Count)
        {
            await Task.Delay(1100);
            PresentationEvents.FadeInOutEvent?.Invoke(true);
            GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Tutorial);

            SaveDataEvents.SaveGameEvent?.Invoke("HangOutScene");
        }
    }

    public async void CheckClearNode()
    {
        _clearNode++;

        if(_clearNode == 9)
        {
            await Task.Delay(1100);
            PresentationEvents.FadeInOutEvent?.Invoke(true);
        }

        _dialogueController.DialogueSkip();
        _dialogueUIController.OnEndTyping(false);
    }

    public void LodingHandle(DataController dataController)
    {
        //dataController.stageData.stageSaveData.stageState = StageState.Clear;

        _dialogueController.dialogueDataReader = dataController.stageData.dialogueData_KR;
        //_dialogueUIController.dialogueDataReader_KR = dataController.stageData.dialogueData_KR;
        //_dialogueUIController.dialogueDataReader_EN = dataController.stageData.dialogueData_EN;

        dataController.SuccessGiveData();
    }
}
