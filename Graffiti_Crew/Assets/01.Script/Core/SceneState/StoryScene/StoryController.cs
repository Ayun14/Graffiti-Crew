using AH.UI.Events;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryController : Observer<GameStateController>, INeedLoding
{
    [SerializeField] private DialogueUIController _dialogueUIController;

    private StoryDialogueSO _storyDialogueSO;
    private int _dialogueNum = 0;

    private Image _loadingPanel;
    private AudioSource _bgm;


    private void Awake()
    {
        Attach();

        Transform canvas = transform.Find("Canvas");
        _loadingPanel = canvas.Find("Panel_Loading").GetComponent<Image>();
    }

    private void OnDestroy()
    {
        Detach();
    }

    public override void NotifyHandle()
    {
        if(mySubject != null)
        {

            _loadingPanel.gameObject.SetActive(mySubject.GameState == GameState.Loding);

            if (mySubject.GameState == GameState.Dialogue)
            {
                _bgm = GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.Fight_After, true);

                _dialogueUIController.ChangeDialogueUI?.Invoke(true);
                StageEvent.SetActiveFightViewEvent?.Invoke(false);
                DialougeEvent.ShowDialougeViewEvent?.Invoke(true);

                NPCSO dialogue = _storyDialogueSO.storyList[_dialogueNum];
                _dialogueUIController.ChangeDialogueUI?.Invoke(true);
                _dialogueUIController.StartDialogue(dialogue.startIndex, dialogue.endIndex, DialogueEnd);
            }
        }
    }

    private async void DialogueEnd()
    {
        PresentationEvents.FadeInOutEvent?.Invoke(false);
        await Task.Delay(1100);
        DialougeEvent.ShowDialougeViewEvent?.Invoke(false);
        _dialogueNum++;


        if (_dialogueNum == _storyDialogueSO.storyList.Count)
        {
            PresentationEvents.FadeInOutEvent?.Invoke(false);
            await Task.Delay(1100);
            GameManager.Instance.SoundSystemCompo.StopLoopSound(SoundType.Fight_After);

            SaveDataEvents.SaveGameEvent?.Invoke("ComputerScene");
        }
        else
        {
            NPCSO dialogue = _storyDialogueSO.storyList[_dialogueNum];
            await Task.Delay(1100);
            PresentationEvents.FadeInOutEvent?.Invoke(true);
            _dialogueUIController.StartDialogue(dialogue.startIndex, dialogue.endIndex, DialogueEnd);
        }

    }

    public void LodingHandle(DataController dataController)
    {
        dataController.stageData.stageSaveData.isClear = true;

        _storyDialogueSO = dataController.stageData.storyDialogue;
        Debug.Log(dataController.stageData.storyDialogue);

        _dialogueUIController.dialogueDataReader = dataController.stageData.dialogueData_KR;
        _dialogueUIController.dialogueDataReader_KR = dataController.stageData.dialogueData_KR;
        _dialogueUIController.dialogueDataReader_EN = dataController.stageData.dialogueData_EN;

        dataController.SuccessGiveData();
    }
}
