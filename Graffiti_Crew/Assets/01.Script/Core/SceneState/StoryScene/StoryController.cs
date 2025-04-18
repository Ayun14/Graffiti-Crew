using AH.UI.Events;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class StoryController : Observer<GameStateController>, INeedLoding
{
    [SerializeField] private DialogueUIController _dialogueUIController;

    private StoryDialogueSO _storyDialogueSO;
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
                GameManager.Instance.SoundSystemCompo.PlayBGM(SoundType.Fight_After);

                _dialogueUIController.ChangeDialogueUI?.Invoke(true);
                StageEvent.SetActiveFightViewEvent?.Invoke(false);

                DialogueEvent.SetCharacterEvent?.Invoke(DialougeCharacter.Jia);
                DialogueEvent.ShowDialougeViewEvent?.Invoke(true);

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
        DialogueEvent.ShowDialougeViewEvent?.Invoke(false);
        _dialogueNum++;


        if (_dialogueNum == _storyDialogueSO.storyList.Count)
        {
            PresentationEvents.FadeInOutEvent?.Invoke(false);
            await Task.Delay(1100);
            GameManager.Instance.SoundSystemCompo.StopBGM(SoundType.Fight_After);

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

        _dialogueUIController.dialogueDataReader = dataController.stageData.dialogueData_KR;
        _dialogueUIController.dialogueDataReader_KR = dataController.stageData.dialogueData_KR;
        _dialogueUIController.dialogueDataReader_EN = dataController.stageData.dialogueData_EN;

        dataController.SuccessGiveData();
    }
}
