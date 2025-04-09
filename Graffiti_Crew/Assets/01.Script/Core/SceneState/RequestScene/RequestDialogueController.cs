using AH.UI.Events;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RequestDialogueController : Observer<GameStateController>, INeedLoding
{
    [SerializeField] private DialogueUIController _dialogueUIController;

    [SerializeField] private StoryDialogueSO _requestDialogueSO;
    private int _dialogueNum = 0;

    private Image _loadingPanel;

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
        if (mySubject != null)
        {
            _loadingPanel.gameObject.SetActive(mySubject.GameState == GameState.Loding);

            if (mySubject.GameState == GameState.Talk)
            {
                StageEvent.SetActiveFightViewEvent?.Invoke(false);

                GameManager.Instance.SoundSystemCompo.PlaySound(SoundType.Request);

                AnimationEvent.SetAnimation?.Invoke(10, AnimationEnum.Talk);
                AnimationEvent.SetAnimation?.Invoke(1, AnimationEnum.Talk);

                NPCSO dialogue = _requestDialogueSO.storyList[_dialogueNum];

                
                _dialogueUIController.ChangeDialogueUI?.Invoke(true);
                if (_dialogueNum == 0)
                    _dialogueUIController.StartDialogue(dialogue.startIndex, dialogue.endIndex, DialogueEnd);
                else
                    EndDialogue();
            }
        }
    }

    private async void EndDialogue()
    {
        NPCSO dialogue = _requestDialogueSO.storyList[_dialogueNum];

        await Task.Delay(1500);
        PresentationEvents.FadeInOutEvent?.Invoke(false);
        _dialogueUIController.StartDialogue(dialogue.startIndex, dialogue.endIndex, DialogueEnd);
        await Task.Delay(2100);
        PresentationEvents.FadeInOutEvent?.Invoke(true);
    }

    private async void DialogueEnd()
    {
        _dialogueNum++;
        if (_dialogueNum == 1)
        {
            PresentationEvents.FadeInOutEvent?.Invoke(false);
            await Task.Delay(2100);
            PresentationEvents.FadeInOutEvent?.Invoke(true);

            mySubject.ChangeGameState(GameState.Graffiti);
        }
        else if (_dialogueNum == 2)
        {
            PresentationEvents.FadeInOutEvent?.Invoke(false);
            await Task.Delay(1100);

            GameManager.Instance.SoundSystemCompo.StopLoopSound(SoundType.Request);
            SaveDataEvents.SaveGameEvent?.Invoke("ComputerScene");
        }

    }

    public void LodingHandle(DataController dataController)
    {
        //dataController.stageData.stageSaveData.isClear = true;

        _requestDialogueSO = dataController.stageData.storyDialogue;

        _dialogueUIController.dialogueDataReader = dataController.stageData.dialogueData_KR;
        _dialogueUIController.dialogueDataReader_KR = dataController.stageData.dialogueData_KR;
        _dialogueUIController.dialogueDataReader_EN = dataController.stageData.dialogueData_EN;

        dataController.SuccessGiveData();
    }
}
