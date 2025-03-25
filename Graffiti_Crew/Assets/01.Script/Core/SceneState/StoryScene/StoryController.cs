using AH.UI.Events;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryController : Observer<GameStateController>, INeedLoding
{
    [SerializeField] private DialogueUIController _dialogueUIController;

    [SerializeField] private List<NPCSO> _dialogueList;
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
        if(mySubject != null)
        {
            _loadingPanel.gameObject.SetActive(mySubject.GameState == GameState.Loding);

            if (mySubject.GameState == GameState.Dialogue)
            {
                _dialogueUIController.ChangeDialogueUI?.Invoke(true);
                StageEvent.SetActiveFightViewEvent?.Invoke(false);
                DialougeEvent.ShowDialougeViewEvent?.Invoke(true);

                NPCSO dialogue = _dialogueList[_dialogueNum];
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


        if (_dialogueNum == _dialogueList.Count)
        {
            PresentationEvents.FadeInOutEvent?.Invoke(false);
            await Task.Delay(1100);
            GameEvents.SaveGameEvent?.Invoke();
            SceneManager.LoadScene("ComputerScene");
        }
        else
        {
            NPCSO dialogue = _dialogueList[_dialogueNum];
            await Task.Delay(1100);
            PresentationEvents.FadeInOutEvent?.Invoke(true);
            _dialogueUIController.StartDialogue(dialogue.startIndex, dialogue.endIndex, DialogueEnd);
        }

    }

    public void LodingHandle(DataController dataController)
    {
        _dialogueUIController.dialogueDataReader = dataController.stageData.dialogueData_KR;
        _dialogueUIController.dialogueDataReader_KR = dataController.stageData.dialogueData_KR;
        _dialogueUIController.dialogueDataReader_EN = dataController.stageData.dialogueData_EN;

        dataController.SuccessGiveData();
    }
}
